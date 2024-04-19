#pragma warning disable CS0660 // 类型定义运算符 == 或运算符 !=，但不重写 Object.Equals(object o)
#pragma warning disable CS0661 // 类型定义运算符 == 或运算符 !=，但不重写 Object.GetHashCode()
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.Netcode;

public class MapGeneratorScript : MonoBehaviour
{
    [Header("MapManager")]
    public GameObject mapManager;

    //地面生成相关
    [Header("地面生成")]
    public Vector2 mapSize;

    //障碍物相关
    public List<Coord> allTilesCoord = new List<Coord>();

    [Header("障碍物生成率")]
    [Range(0, 0.6f)] public float obsPercent;
    private Coord mapCenter;

    public int[,] mapInformation;

    private Queue<Coord> shuffleQueue;

    public void SpawnMap()
    {
        Debug.Log("生成地图中");
        MapManager mapManagerScript = mapManager.GetComponent<MapManager>();

        mapInformation = GenerateMap();

        mapManagerScript.netWorkMapInformation.Value = new NetWorkMapInformation(mapInformation);
        mapManagerScript.mapSizeX.Value = mapInformation.GetLength(0);
        mapManagerScript.mapSizeY.Value = mapInformation.GetLength(1);
        mapManagerScript.SpawnMap();
    }

    private int[,] GenerateMap()
    {
        //生成一个二维数组
        int[,] mapInformation = new int[(int)mapSize.x + 1, (int)mapSize.y + 1];//初始值为0
        /*
            0:地面
            1 - 5:山
            6:池塘
            7:不可破坏地面
            8:不可破坏障碍
         */
        bool[,] mapFlag = new bool[(int)mapSize.x, (int)mapSize.y];//初始值为false
        mapCenter = new Coord((int)mapSize.x / 2, (int)mapSize.y / 2);//设定地图中心点

        //生成不可破坏墙和不可破坏地面（出生点和点）
        bool[,] cannotSpawnPlace = new bool[(int)mapSize.x + 1,(int)mapSize.y + 1];
        //初始化数组：将出生点和点确定
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

        //打乱后的序列（对地图地块进行洗牌）
        shuffleQueue = new Queue<Coord>(Utilities.ShuffleCroods(allTilesCoord.ToArray()));

        //根据洗牌后的队列生成障碍区队列
        Queue<ObsEreaInfo> obsEreaQueue = new Queue<ObsEreaInfo>();
        for (int i = 0; i < shuffleQueue.Count; i++)
        {
            Coord nowCoord = shuffleQueue.Dequeue();
            obsEreaQueue.Enqueue(new ObsEreaInfo(nowCoord.x, nowCoord.y));
        }

        Debug.Log("生成障碍物区队列成功");


        //根据队列生成障碍物
        int obsCount = (int)(mapSize.x * mapSize.y * obsPercent);
        int currentObsCount = 0;
        while (currentObsCount <= obsCount)
        {
            Debug.Log("正在进行一个障碍区的生成");
            //获取一个将要生成的随机障碍物区
            ObsEreaInfo randomObsErea = obsEreaQueue.Dequeue();
            while (!(randomObsErea.x + randomObsErea.sizeX/2 < mapInformation.GetLength(0) && randomObsErea.y + randomObsErea.sizeY/2 < mapInformation.GetLength(1)
                && randomObsErea.x - randomObsErea.sizeX/2 > 0 && randomObsErea.y - randomObsErea.sizeY/2 > 0 && mapInformation[randomObsErea.x, randomObsErea.y] == 0))
            {
                Debug.Log("出界！重新抽取");
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

            //生成不同的障碍物区
            switch (randomObsErea.terrainIndex)
            {
                /*
                 1：山区
                 2：水域
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
                //生成地图，这个应该移到MapSpawner脚本中
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


                //打乱后的序列（对地图地块进行洗牌）
                shuffleQueue = new Queue<Coord>(Utilities.ShuffleCroods(allTilesCoord.ToArray()));
                //遍历obsCount次，每次遍历从打乱后的序列中取一个地块作为随机地块
                int obsCount = (int)(mapSize.x * mapSize.y * obsPercent);
                mapCenter = new Coord((int)mapSize.x / 2, (int)mapSize.y / 2);//设定地图中心点
                mapObstacles = new bool[(int)mapSize.x, (int)mapSize.y];//初始化obs信息二维数组

                int currentObsCount = 0;

                for (int i = 0; i < obsCount; i++)
                {
                    Coord randomCoord = shuffleQueue.Dequeue();
                    shuffleQueue.Enqueue(randomCoord);

                    //假设有障碍物
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

                //创建空气墙
        */

        return mapInformation;
    }

    private int[,] SpawnMountains(int[,] _mapInformation,ObsEreaInfo _randomObsErea,ref int _currentObsCount)
    {
        Debug.Log("进入生成山脉的函数");
        int inEreaObsCount = 0;
        int inEreaTotalCount = (int)(_randomObsErea.sizeX * _randomObsErea.sizeY * _randomObsErea.obsPercent);
        Coord inEreaLeftDownCoord = new Coord(_randomObsErea.x - _randomObsErea.sizeX / 2, _randomObsErea.y - _randomObsErea.sizeY / 2);
        Coord inEreaRightUpCoord = new Coord(_randomObsErea.x + _randomObsErea.sizeX / 2, _randomObsErea.y + _randomObsErea.sizeY / 2);
        Queue<Coord> inEreaShuffleQueue = new Queue<Coord>(Utilities.ShuffleCroods(_randomObsErea));
        for (int i = 0; i < inEreaTotalCount; i++)
        {
            Debug.Log("inEreaTotalCount：" + inEreaTotalCount.ToString());

            //保证障碍区不会出界
            Coord inEreaRandomCoord = inEreaShuffleQueue.Dequeue();
            inEreaShuffleQueue.Enqueue(inEreaRandomCoord);

            //生成当前障碍物区的小障碍物map，用于洪水填充算法

            //只在可通行的地方生成障碍物
            Debug.Log((inEreaLeftDownCoord.x + inEreaRandomCoord.x).ToString() + "," + (inEreaLeftDownCoord.y + inEreaRandomCoord.y).ToString());
            Debug.Log(inEreaLeftDownCoord.x.ToString());
            Debug.Log(inEreaRandomCoord.x.ToString());
            Debug.Log(inEreaLeftDownCoord.y.ToString());
            Debug.Log(inEreaRandomCoord.y.ToString());
            if (_mapInformation[inEreaLeftDownCoord.x + inEreaRandomCoord.x, inEreaLeftDownCoord.y + inEreaRandomCoord.y] == 0) //TODO 数组越界
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
                    //生成障碍物
                    _mapInformation[inEreaLeftDownCoord.x + inEreaRandomCoord.x, inEreaLeftDownCoord.y + inEreaRandomCoord.y] = UnityEngine.Random.Range(1,5);
                    inEreaObsCount++;
                    _currentObsCount++;
                    //Debug.Log("当前障碍物区" + _randomObsErea.x.ToString() + "," + _randomObsErea.y.ToString() + "已生成" + inEreaObsCount.ToString() + "个障碍物");
                }

            }

        }
        return _mapInformation;
    }

    private int[,] SpawnWaters(int[,] _mapInformation,ObsEreaInfo _randomObsErea, ref int _obsCount)
    {
        Debug.Log("进入生成水域的函数");

        int inEreaObsCount = 0;
        int inEreaTotalCount = (int)(_randomObsErea.sizeX * _randomObsErea.sizeY * _randomObsErea.obsPercent);
        Coord inEreaLeftDownCoord = new Coord(_randomObsErea.x - _randomObsErea.sizeX / 2, _randomObsErea.y - _randomObsErea.sizeY / 2);
        Coord inEreaRightUpCoord = new Coord(_randomObsErea.x + _randomObsErea.sizeX / 2, _randomObsErea.y + _randomObsErea.sizeY / 2);
        Coord inEreaCenterCoord = new Coord(_randomObsErea.x, _randomObsErea.y);

        Queue<Coord> coordQueue = new Queue<Coord>(); //队列用于先进先出遍历

        coordQueue.Enqueue(inEreaCenterCoord);    //从中心点开始遍历
        _mapInformation[inEreaCenterCoord.x, inEreaCenterCoord.y] = 6;  //将障碍区中心视为已遍历
        inEreaObsCount++;
        _obsCount++;

        while (inEreaObsCount < inEreaTotalCount && coordQueue.Count > 0)
        {
            Coord currentCoord = coordQueue.Dequeue();

            //获取周围的瓦片
            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    Coord neighborCoord = new Coord(currentCoord.x + x, currentCoord.y + y);

                    if (neighborCoord.x >= inEreaLeftDownCoord.x && neighborCoord.x <= inEreaRightUpCoord.x
                       && neighborCoord.y >= inEreaLeftDownCoord.y && neighborCoord.y <= inEreaRightUpCoord.y) //没有超出障碍物区边界
                    {
                        if (_mapInformation[neighborCoord.x, neighborCoord.y] != 6 && _mapInformation[neighborCoord.x,neighborCoord.y] == 0)//TODO未找到实例
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

    //洪水填充算法
    private bool MapIsFullyAccessible(int[,] _mapInformation, int _currentObsCount,Coord _inEreaLeftDownCoord,Coord _inEreaRightUpCoord, ObsEreaInfo _randomObsErea)
    {
        bool[,] mapFlags = new bool[_randomObsErea.sizeX, _randomObsErea.sizeY]; //地图标志数组，用于标记已遍历地块
        Queue<Coord> coordQueue = new Queue<Coord>(); //队列用于先进先出遍历
        Coord _inEreaCenterCoord = new Coord(_inEreaLeftDownCoord.x + _randomObsErea.sizeX / 2, _inEreaLeftDownCoord.y + _randomObsErea.sizeY / 2);
        coordQueue.Enqueue(_inEreaCenterCoord);    //从中心点开始遍历
        mapFlags[(int)(_randomObsErea.sizeX / 2), (int)(_randomObsErea.sizeX / 2)] = true;  //将地图中心设置为已遍历

        int accessibleCount = 1;

        while(coordQueue.Count > 0)
        {
            Coord currentCoord = coordQueue.Dequeue();

            //获取周围的瓦片
            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    Coord neighborCoord = new Coord(currentCoord.x + x, currentCoord.y + y);

                    if(x == 0 || y == 0)//排除对角
                    {
                        if(neighborCoord.x >= 0 && neighborCoord.x < _randomObsErea.sizeX
                           && neighborCoord.y >=0 && neighborCoord.y < _randomObsErea.sizeY) //没有超出障碍物区边界
                        {
                            if (!mapFlags[neighborCoord.x, neighborCoord.y] && _mapInformation[_inEreaLeftDownCoord.x + neighborCoord.x, _inEreaLeftDownCoord.y + neighborCoord.y] == 0) //假设没有障碍物 //TODO未找到实例
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

    //基础构造函数
    public ObsEreaInfo (int _x,int _y, int _sizeX, int _sizeY, float _obsPercent, int _terrainIndex)
    {
        x = _x;
        y = _y;
        sizeX = _sizeX;
        sizeY = _sizeY;
        obsPercent = _obsPercent;
        terrainIndex = _terrainIndex;
    }

    //构造函数重载
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