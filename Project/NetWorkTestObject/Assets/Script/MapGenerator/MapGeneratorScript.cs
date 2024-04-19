#pragma warning disable CS0660 // ���Ͷ�������� == ������� !=��������д Object.Equals(object o)
#pragma warning disable CS0661 // ���Ͷ�������� == ������� !=��������д Object.GetHashCode()
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.Netcode;

public class MapGeneratorScript : MonoBehaviour
{
    [Header("MapManager")]
    public GameObject mapManager;

    //�����������
    [Header("��������")]
    public Vector2 mapSize;

    //�ϰ������
    public List<Coord> allTilesCoord = new List<Coord>();

    [Header("�ϰ���������")]
    [Range(0, 0.6f)] public float obsPercent;
    private Coord mapCenter;

    public int[,] mapInformation;

    private Queue<Coord> shuffleQueue;

    public void SpawnMap()
    {
        Debug.Log("���ɵ�ͼ��");
        MapManager mapManagerScript = mapManager.GetComponent<MapManager>();

        mapInformation = GenerateMap();

        mapManagerScript.netWorkMapInformation.Value = new NetWorkMapInformation(mapInformation);
        mapManagerScript.mapSizeX.Value = mapInformation.GetLength(0);
        mapManagerScript.mapSizeY.Value = mapInformation.GetLength(1);
        mapManagerScript.SpawnMap();
    }

    private int[,] GenerateMap()
    {
        //����һ����ά����
        int[,] mapInformation = new int[(int)mapSize.x + 1, (int)mapSize.y + 1];//��ʼֵΪ0
        /*
            0:����
            1 - 5:ɽ
            6:����
            7:�����ƻ�����
            8:�����ƻ��ϰ�
         */
        bool[,] mapFlag = new bool[(int)mapSize.x, (int)mapSize.y];//��ʼֵΪfalse
        mapCenter = new Coord((int)mapSize.x / 2, (int)mapSize.y / 2);//�趨��ͼ���ĵ�

        //���ɲ����ƻ�ǽ�Ͳ����ƻ����棨������͵㣩
        bool[,] cannotSpawnPlace = new bool[(int)mapSize.x + 1,(int)mapSize.y + 1];
        //��ʼ�����飺��������͵�ȷ��
        for (int i = 0; i < mapSize.x + 1; i++)
        {
            for (int j = 0; j < mapSize.y + 1; j++)
            {
                if (i >= 1 && i <= 4 && j >= 1 && j <= 4) mapInformation[i, j] = 7;
                if (i <= mapSize.x - 1 && i >= mapSize.x - 4 && j <= mapSize.y - 1 && j >= mapSize.y - 4) mapInformation[i, j] = 7;
                if (i >= mapCenter.x - 1 && i <= mapCenter.x + 1 && j >= mapCenter.y - 1 && j <= mapCenter.y + 1) mapInformation[i, j] = 7;
                if (i == 0 || j == 0 || i == mapSize.x || j == mapSize.y) mapInformation[i,j] = 8;


                allTilesCoord.Add(new Coord(i, j));
            }
        }

        //���Һ�����У��Ե�ͼ�ؿ����ϴ�ƣ�
        shuffleQueue = new Queue<Coord>(Utilities.ShuffleCroods(allTilesCoord.ToArray()));

        //����ϴ�ƺ�Ķ��������ϰ�������
        Queue<ObsEreaInfo> obsEreaQueue = new Queue<ObsEreaInfo>();
        for (int i = 0; i < shuffleQueue.Count; i++)
        {
            Coord nowCoord = shuffleQueue.Dequeue();
            obsEreaQueue.Enqueue(new ObsEreaInfo(nowCoord.x, nowCoord.y));
        }

        Debug.Log("�����ϰ��������гɹ�");


        //���ݶ��������ϰ���
        int obsCount = (int)(mapSize.x * mapSize.y * obsPercent);
        int currentObsCount = 0;
        while (currentObsCount <= obsCount)
        {
            Debug.Log("���ڽ���һ���ϰ���������");
            //��ȡһ����Ҫ���ɵ�����ϰ�����
            ObsEreaInfo randomObsErea = obsEreaQueue.Dequeue();
            while (!(randomObsErea.x + randomObsErea.sizeX/2 < mapInformation.GetLength(0) && randomObsErea.y + randomObsErea.sizeY/2 < mapInformation.GetLength(1)
                && randomObsErea.x - randomObsErea.sizeX/2 > 0 && randomObsErea.y - randomObsErea.sizeY/2 > 0 && mapInformation[randomObsErea.x, randomObsErea.y] == 0))
            {
                Debug.Log("���磡���³�ȡ");
                if (obsEreaQueue.Count == 0)
                {
                    shuffleQueue = new Queue<Coord>(Utilities.ShuffleCroods(allTilesCoord.ToArray()));
                    obsEreaQueue = new Queue<ObsEreaInfo>();
                    for (int i = 0; i < shuffleQueue.Count; i++)
                    {
                        Coord nowCoord = shuffleQueue.Dequeue();
                        obsEreaQueue.Enqueue(new ObsEreaInfo(nowCoord.x, nowCoord.y));
                    }
                }
                randomObsErea = obsEreaQueue.Dequeue();
            }

            //���ɲ�ͬ���ϰ�����
            switch (randomObsErea.terrainIndex)
            {
                /*
                 1��ɽ��
                 2��ˮ��
                 */
                case 1:
                    mapInformation = SpawnMountains(mapInformation ,randomObsErea, ref currentObsCount);
                    break;
                case 2:
                    mapInformation = SpawnWaters(mapInformation, randomObsErea, ref currentObsCount);
                    break;
            }
        }


        /*
                //���ɵ�ͼ�����Ӧ���Ƶ�MapSpawner�ű���
                for (int i = 0; i < mapSize.x; i++)
                {
                    for (int j = 0; j < mapSize.y; j++)
                    {
                        Vector3 newPos = new Vector3(-mapSize.x / 2 + 0.5f + i, -0.5f, -mapSize.y / 2 + 0.5f + j);
                        GameObject spawnTile = Instantiate(tilePrefab, newPos, Quaternion.identity);
                        spawnTile.transform.SetParent(mapHoleder);
                        spawnTile.transform.localScale *= 1;

                        allTilesCoord.Add(new Coord(i, j));
                    }
                }


                //���Һ�����У��Ե�ͼ�ؿ����ϴ�ƣ�
                shuffleQueue = new Queue<Coord>(Utilities.ShuffleCroods(allTilesCoord.ToArray()));
                //����obsCount�Σ�ÿ�α����Ӵ��Һ��������ȡһ���ؿ���Ϊ����ؿ�
                int obsCount = (int)(mapSize.x * mapSize.y * obsPercent);
                mapCenter = new Coord((int)mapSize.x / 2, (int)mapSize.y / 2);//�趨��ͼ���ĵ�
                mapObstacles = new bool[(int)mapSize.x, (int)mapSize.y];//��ʼ��obs��Ϣ��ά����

                int currentObsCount = 0;

                for (int i = 0; i < obsCount; i++)
                {
                    Coord randomCoord = shuffleQueue.Dequeue();
                    shuffleQueue.Enqueue(randomCoord);

                    //�������ϰ���
                    mapObstacles[randomCoord.x, randomCoord.y] = true;
                    currentObsCount++;

                    if(randomCoord != mapCenter && MapIsFullyAccessible(mapObstacles,currentObsCount))
                    {
                        float obsHeight = UnityEngine.Random.Range(minHeight, maxHeight);

                        Vector3 newPos = new Vector3(-mapSize.x / 2 + 0.5f + randomCoord.x, obsHeight / 2f, -mapSize.y / 2 + 0.5f + randomCoord.y);
                        GameObject spawnObs = Instantiate(obsPrefab, newPos, Quaternion.identity);
                        spawnObs.transform.SetParent(mapHoleder);
                        spawnObs.transform.localScale = new Vector3(1, obsHeight, 1);

                        MeshRenderer meshRender = spawnObs.GetComponent<MeshRenderer>();
                        Material material = meshRender.material;

                        float colorPercent = randomCoord.y / mapSize.y;
                        meshRender.material.color = Color.Lerp(foreGroundColor, backGroundColor, colorPercent);
                    }
                    else
                    {
                        mapObstacles[randomCoord.x, randomCoord.y] = false;
                        currentObsCount--;
                    }
                }

                //��������ǽ
        */

        return mapInformation;
    }

    private int[,] SpawnMountains(int[,] _mapInformation,ObsEreaInfo _randomObsErea,ref int _currentObsCount)
    {
        Debug.Log("��������ɽ���ĺ���");
        int inEreaObsCount = 0;
        int inEreaTotalCount = (int)(_randomObsErea.sizeX * _randomObsErea.sizeY * _randomObsErea.obsPercent);
        Coord inEreaLeftDownCoord = new Coord(_randomObsErea.x - _randomObsErea.sizeX / 2, _randomObsErea.y - _randomObsErea.sizeY / 2);
        Coord inEreaRightUpCoord = new Coord(_randomObsErea.x + _randomObsErea.sizeX / 2, _randomObsErea.y + _randomObsErea.sizeY / 2);
        Queue<Coord> inEreaShuffleQueue = new Queue<Coord>(Utilities.ShuffleCroods(_randomObsErea));
        for (int i = 0; i < inEreaTotalCount; i++)
        {
            Debug.Log("inEreaTotalCount��" + inEreaTotalCount.ToString());

            //��֤�ϰ����������
            Coord inEreaRandomCoord = inEreaShuffleQueue.Dequeue();
            inEreaShuffleQueue.Enqueue(inEreaRandomCoord);

            //���ɵ�ǰ�ϰ�������С�ϰ���map�����ں�ˮ����㷨

            //ֻ�ڿ�ͨ�еĵط������ϰ���
            Debug.Log((inEreaLeftDownCoord.x + inEreaRandomCoord.x).ToString() + "," + (inEreaLeftDownCoord.y + inEreaRandomCoord.y).ToString());
            Debug.Log(inEreaLeftDownCoord.x.ToString());
            Debug.Log(inEreaRandomCoord.x.ToString());
            Debug.Log(inEreaLeftDownCoord.y.ToString());
            Debug.Log(inEreaRandomCoord.y.ToString());
            if (_mapInformation[inEreaLeftDownCoord.x + inEreaRandomCoord.x, inEreaLeftDownCoord.y + inEreaRandomCoord.y] == 0) //TODO ����Խ��
            {
                if (MapIsFullyAccessible(mapInformation, inEreaObsCount, inEreaLeftDownCoord, inEreaRightUpCoord,_randomObsErea))
                {
                    /*
                    float obsHeight = UnityEngine.Random.Range(minHeight, maxHeight);

                    Vector3 newPos = new Vector3(-mapSize.x / 2 + 0.5f + randomCoord.x, obsHeight / 2f, -mapSize.y / 2 + 0.5f + randomCoord.y);
                    GameObject spawnObs = Instantiate(obsPrefab, newPos, Quaternion.identity);
                    spawnObs.transform.SetParent(mapHoleder);
                    spawnObs.transform.localScale = new Vector3(1, obsHeight, 1);

                    MeshRenderer meshRender = spawnObs.GetComponent<MeshRenderer>();
                    Material material = meshRender.material;

                    float colorPercent = randomCoord.y / mapSize.y;
                    meshRender.material.color = Color.Lerp(foreGroundColor, backGroundColor, colorPercent);
                    */
                }
                else
                {
                    //�����ϰ���
                    _mapInformation[inEreaLeftDownCoord.x + inEreaRandomCoord.x, inEreaLeftDownCoord.y + inEreaRandomCoord.y] = UnityEngine.Random.Range(1,5);
                    inEreaObsCount++;
                    _currentObsCount++;
                    //Debug.Log("��ǰ�ϰ�����" + _randomObsErea.x.ToString() + "," + _randomObsErea.y.ToString() + "������" + inEreaObsCount.ToString() + "���ϰ���");
                }

            }

        }
        return _mapInformation;
    }

    private int[,] SpawnWaters(int[,] _mapInformation,ObsEreaInfo _randomObsErea, ref int _obsCount)
    {
        Debug.Log("��������ˮ��ĺ���");

        int inEreaObsCount = 0;
        int inEreaTotalCount = (int)(_randomObsErea.sizeX * _randomObsErea.sizeY * _randomObsErea.obsPercent);
        Coord inEreaLeftDownCoord = new Coord(_randomObsErea.x - _randomObsErea.sizeX / 2, _randomObsErea.y - _randomObsErea.sizeY / 2);
        Coord inEreaRightUpCoord = new Coord(_randomObsErea.x + _randomObsErea.sizeX / 2, _randomObsErea.y + _randomObsErea.sizeY / 2);
        Coord inEreaCenterCoord = new Coord(_randomObsErea.x, _randomObsErea.y);

        Queue<Coord> coordQueue = new Queue<Coord>(); //���������Ƚ��ȳ�����

        coordQueue.Enqueue(inEreaCenterCoord);    //�����ĵ㿪ʼ����
        _mapInformation[inEreaCenterCoord.x, inEreaCenterCoord.y] = 6;  //���ϰ���������Ϊ�ѱ���
        inEreaObsCount++;
        _obsCount++;

        while (inEreaObsCount < inEreaTotalCount && coordQueue.Count > 0)
        {
            Coord currentCoord = coordQueue.Dequeue();

            //��ȡ��Χ����Ƭ
            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    Coord neighborCoord = new Coord(currentCoord.x + x, currentCoord.y + y);

                    if (neighborCoord.x >= inEreaLeftDownCoord.x && neighborCoord.x <= inEreaRightUpCoord.x
                       && neighborCoord.y >= inEreaLeftDownCoord.y && neighborCoord.y <= inEreaRightUpCoord.y) //û�г����ϰ������߽�
                    {
                        if (_mapInformation[neighborCoord.x, neighborCoord.y] != 6 && _mapInformation[neighborCoord.x,neighborCoord.y] == 0)//TODOδ�ҵ�ʵ��
                        {
                                coordQueue.Enqueue(neighborCoord);
                                _mapInformation[neighborCoord.x, neighborCoord.y] = 6;
                                inEreaObsCount++;
                                _obsCount++;
                        }
                    }
                }
            }
        }

        return _mapInformation;
    }

    //��ˮ����㷨
    private bool MapIsFullyAccessible(int[,] _mapInformation, int _currentObsCount,Coord _inEreaLeftDownCoord,Coord _inEreaRightUpCoord, ObsEreaInfo _randomObsErea)
    {
        bool[,] mapFlags = new bool[_randomObsErea.sizeX, _randomObsErea.sizeY]; //��ͼ��־���飬���ڱ���ѱ����ؿ�
        Queue<Coord> coordQueue = new Queue<Coord>(); //���������Ƚ��ȳ�����
        Coord _inEreaCenterCoord = new Coord(_inEreaLeftDownCoord.x + _randomObsErea.sizeX / 2, _inEreaLeftDownCoord.y + _randomObsErea.sizeY / 2);
        coordQueue.Enqueue(_inEreaCenterCoord);    //�����ĵ㿪ʼ����
        mapFlags[(int)(_randomObsErea.sizeX / 2), (int)(_randomObsErea.sizeX / 2)] = true;  //����ͼ��������Ϊ�ѱ���

        int accessibleCount = 1;

        while(coordQueue.Count > 0)
        {
            Coord currentCoord = coordQueue.Dequeue();

            //��ȡ��Χ����Ƭ
            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    Coord neighborCoord = new Coord(currentCoord.x + x, currentCoord.y + y);

                    if(x == 0 || y == 0)//�ų��Խ�
                    {
                        if(neighborCoord.x >= 0 && neighborCoord.x < _randomObsErea.sizeX
                           && neighborCoord.y >=0 && neighborCoord.y < _randomObsErea.sizeY) //û�г����ϰ������߽�
                        {
                            if (!mapFlags[neighborCoord.x, neighborCoord.y] && _mapInformation[_inEreaLeftDownCoord.x + neighborCoord.x, _inEreaLeftDownCoord.y + neighborCoord.y] == 0) //����û���ϰ��� //TODOδ�ҵ�ʵ��
                            {
                                coordQueue.Enqueue(neighborCoord);
                                mapFlags[neighborCoord.x, neighborCoord.y] = true;
                                accessibleCount++;
                            }
                        }
                    }
                }
            }
        }

        int obsTargetCount = (int)(mapSize.x * mapSize.y - _currentObsCount);
        return accessibleCount == obsTargetCount;
    }

}

[System.Serializable]
public struct Coord
{
    public int x;
    public int y;
    public Coord(int _x, int _y)
    {
        this.x = _x;
        this.y = _y;
    }

    public static bool operator !=(Coord _c1, Coord _c2)
    {
        return !(_c1 == _c2);
    }
    public static bool operator ==(Coord _c1, Coord _c2)
    {
        return (_c1.x == _c2.x) && (_c1.y == _c2.y);
    }
}

public struct ObsEreaInfo
{
    public int x;
    public int y;
    public int sizeX;
    public int sizeY;
    public float obsPercent;
    public int terrainIndex;

    //�������캯��
    public ObsEreaInfo (int _x,int _y, int _sizeX, int _sizeY, float _obsPercent, int _terrainIndex)
    {
        x = _x;
        y = _y;
        sizeX = _sizeX;
        sizeY = _sizeY;
        obsPercent = _obsPercent;
        terrainIndex = _terrainIndex;
    }

    //���캯������
    public ObsEreaInfo(int _x, int _y)
    {
        x = _x;
        y = _y;
        sizeX = UnityEngine.Random.Range(3, 5);
        sizeY = UnityEngine.Random.Range(3, 5);
        obsPercent = UnityEngine.Random.Range(0f, 1f);
        terrainIndex = UnityEngine.Random.Range(1, 3);
    }
}