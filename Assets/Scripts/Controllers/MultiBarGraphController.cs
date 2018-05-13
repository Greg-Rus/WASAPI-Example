using UniRx;
using UnityEngine;

public class MultiBarGraphController
{
    private CubeView[,] _bars;
    private readonly FrameModel2D _multiFrameModel;
    private readonly BarFactory _barFactory;

    public MultiBarGraphController(FrameModel2D multiFrameModel, BarFactory barFactory)
    {
        _multiFrameModel = multiFrameModel;
        _barFactory = barFactory;
        Setup();
    }


    public void Setup()
    {
        _bars = new CubeView[_multiFrameModel.NumberOfBars, _multiFrameModel.NumberOfBars];
        MakeBars();
        InitializeReactiveBarSubscriptions();
    }

    private void MakeBars()
    {
        _bars = new CubeView[_multiFrameModel.NumberOfBars, _multiFrameModel.NumberOfBars];
        for (int i = 0; i < _multiFrameModel.NumberOfBars; i++)
        {
            for(int j = 0; j < _multiFrameModel.NumberOfBars; j++)
                _bars[i,j] = _barFactory.MakeBarAtPosition(new Vector3(i, 0, j));
        }
    }

    private void InitializeReactiveBarSubscriptions()
    {
        for (int i = 0; i < _multiFrameModel.NumberOfBars; i++)
        {
            for (int j = 0; j < _multiFrameModel.NumberOfBars; j++)
            {
                var rowIndex = i;
                var columnIndex = j;
                _multiFrameModel.GetReactiveBarAtIndex(i,j).Subscribe(curData =>
                {
                    _bars[rowIndex,columnIndex].transform.position = new Vector3(rowIndex, curData / 2.0f * 10.0f, columnIndex);
                    _bars[rowIndex, columnIndex].transform.localScale = new Vector3(1, curData * 10.0f, 1);
                });
            }
        }
    }
}