using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utilities
{
    public static Coord[] ShuffleCroods(Coord[] _dataArray)
    {
        for (int i = 0; i < _dataArray.Length; i++)
        {
            //获取随机数
            int randonNum = Random.Range(i, _dataArray.Length);
            Coord temp = _dataArray[randonNum];
            //互换
            _dataArray[randonNum] = _dataArray[i];
            _dataArray[i] = temp;
        }

        return _dataArray;
    }

    public static Coord[] ShuffleCroods(ObsEreaInfo _randomObsErea)
    {
        List<Coord> inEreaTileQueue = new List<Coord>();
        //对内部地块进行洗牌
        for (int i = 0; i < _randomObsErea.sizeX; i++)
        {
            for (int j = 0; j < _randomObsErea.sizeY; j++)
            {
                inEreaTileQueue.Add(new Coord(i, j));
            }
        }
        return ShuffleCroods(inEreaTileQueue.ToArray());
    }
}
