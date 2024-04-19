using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class MapManager : NetworkBehaviour
{
    [Header("MapManager")]
    public GameObject mapGeneratorScript;

    public GameObject tilePrefab;
    public GameObject cantDestroyTilePrefab;
    public GameObject obsPrefab;
    public Transform mapHoleder;

    [Header("�ϰ�����ɫ")]
    public Color foreGroundColor, backGroundColor;

    public NetworkVariable<NetWorkMapInformation> netWorkMapInformation = new NetworkVariable<NetWorkMapInformation>(writePerm: NetworkVariableWritePermission.Server);
    public NetworkVariable<int> mapSizeX = new NetworkVariable<int>(writePerm: NetworkVariableWritePermission.Server);
    public NetworkVariable<int> mapSizeY = new NetworkVariable<int>(writePerm: NetworkVariableWritePermission.Server);


    //����˵��ÿͻ��˵����ɵ�ͼ�����������ͼ��С��Ϣ���������ɿͻ��˽�ϵ�ͼ��С��Ϣ�����������ͼ��Ϣ���ɵ�ͼ

    
    public void SpawnMap()
    {
        int _mapSizeX = mapSizeX.Value;
        int _mapSizeY = mapSizeY.Value;
        int[,] _mapInformation = netWorkMapInformation.Value.getTypeInformation(_mapSizeX, _mapSizeY);
        /*
        0:����
        1 - 5:ɽ
        6:����
        7:�����ƻ�����
        8:�����ƻ��ϰ�
        */
        Vector2 mapSize = new Vector2(_mapInformation.GetLength(0), _mapInformation.GetLength(1));
        for (int x = 0; x < _mapInformation.GetLength(0); x++)
        {
            for (int y = 0; y < _mapInformation.GetLength(1); y++)
            {
                Vector3 newPos = new Vector3(-mapSize.x / 2 + 0.5f + x, -0.5f, -mapSize.y / 2 + 0.5f + y);
                switch (_mapInformation[x, y])
                {
                    case 0:
                        Instantiate(tilePrefab, newPos, Quaternion.identity).transform.SetParent(mapHoleder);
                        break;
                    case 1:
                    case 2:
                    case 3:
                    case 4:
                    case 5:
                        InstantiateMountain(newPos, _mapInformation[x, y], mapSize);
                        break;
                    case 6:
                        break;
                    case 7:
                        Instantiate(cantDestroyTilePrefab, newPos, Quaternion.identity).transform.SetParent(mapHoleder);
                        break;
                    case 8:
                        Instantiate(cantDestroyTilePrefab, newPos, Quaternion.identity).transform.SetParent(mapHoleder);
                        break;
                }
            }
        }
    }

    private void InstantiateMountain(Vector3 _newPos, int _mountainHeight, Vector2 _mapSize)
    {
        float obsHeight = (_mountainHeight + 1) * 0.5f;

        GameObject spawnObs = Instantiate(obsPrefab, _newPos, Quaternion.identity);
        spawnObs.transform.SetParent(mapHoleder);
        spawnObs.transform.localScale = new Vector3(1, obsHeight, 1);

        MeshRenderer meshRender = spawnObs.GetComponent<MeshRenderer>();
        Material material = meshRender.material;

        float colorPercent = _newPos.y / _mapSize.y;
        meshRender.material.color = Color.Lerp(foreGroundColor, backGroundColor, colorPercent);
    }
}


//NetWorkMapInformation���ھ���ͬ����ͼ��Ϣ
public struct NetWorkMapInformation : INetworkSerializable
{
    private byte[] netTypeInformation;
    private byte[] netHealthInformation;
    private byte[] netConstructInformation;

    public int[,] getTypeInformation(int mapSizeX,int mapSizeY)
    {
        int[,] typeInformation = new int[mapSizeX, mapSizeY];
        for (int i = 0; i < mapSizeX; i++)
        {
            for (int j = 0; j < mapSizeY; j++)
            {
                typeInformation[i, j] = netTypeInformation[i * mapSizeX + j];
            }
        }
        return typeInformation;
    }
    public int[,] getHealthInformation(int mapSizeX,int mapSizeY)
    {
        int[,] healthInformaiton = new int[mapSizeX, mapSizeY];
        for (int i = 0; i < mapSizeX; i++)
        {
            for (int j = 0; j < mapSizeY; j++)
            {
                healthInformaiton[i, j] = netHealthInformation[i * mapSizeX + j];
            }
        }
        return healthInformaiton;
    }
    public int[,] getConstructInformation(int mapSizeX, int mapSizeY)
    {
        int[,] constructInformation = new int[mapSizeX, mapSizeY];
        for (int i = 0; i < mapSizeX; i++)
        {
            for (int j = 0; j < mapSizeY; j++)
            {
                constructInformation[i, j] = netConstructInformation[i * mapSizeX + j];
            }
        }
        return constructInformation;
    }

    //TODO ����set����

    public NetWorkMapInformation(int[,] _mapInformation)
    {
        int mapSizeX = _mapInformation.GetLength(0);
        int mapSizeY = _mapInformation.GetLength(1);

        netTypeInformation = new byte[mapSizeX * mapSizeY];
        netHealthInformation = new byte[mapSizeX * mapSizeY];
        netConstructInformation = new byte[mapSizeX * mapSizeY];

        for (int i = 0; i < mapSizeX; i++)
        {
            for (int j = 0; j < mapSizeY; j++)
            {
                netTypeInformation[i * mapSizeX + j] = (byte)_mapInformation[i,j];
                netHealthInformation[i * mapSizeX + j] = 100;
                netConstructInformation[i * mapSizeX + j] = 100;
            }
        }
    }

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref netTypeInformation);
        serializer.SerializeValue(ref netHealthInformation);
        serializer.SerializeValue(ref netConstructInformation);
    }
}