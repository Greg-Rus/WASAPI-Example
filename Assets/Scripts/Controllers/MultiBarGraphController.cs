using UniRx;
using UnityEngine;

public class MultiBarGraphController
{
    private CubeView[,] _bars;
    private readonly FrameModel2D _multiFrameModel;
    private readonly BarFactory _barFactory;
    private readonly Configuration _configuration;

    public MultiBarGraphController(FrameModel2D multiFrameModel, BarFactory barFactory, Configuration configuration)
    {
        _multiFrameModel = multiFrameModel;
        _barFactory = barFactory;
        _configuration = configuration;
        Setup();
    }


    public void Setup()
    {
        MakeBars();
        InitializeReactiveBarSubscriptions();
    }

    private void MakeBars()
    {
        _bars = new CubeView[BarGridWidth, BarGridLength];
        for (int i = 0; i < BarGridWidth; i++)
        {
            for(int j = 0; j < BarGridLength; j++)
                _bars[i,j] = _barFactory.MakeBarAtPosition(new Vector3(i, 0, j));
        }
    }

    private void InitializeReactiveBarSubscriptions()
    {
        for (int i = 0; i < BarGridWidth; i++)
        {
            for (int j = 0; j < BarGridLength; j++)
            {
                var rowIndex = i;
                var columnIndex = j;
                _multiFrameModel.GetReactiveBarAtIndex(i,j).Subscribe(curData =>
                {
                    UpdateBarViews(rowIndex, columnIndex, curData);
                });
            }
        }
    }

    private void UpdateBarViews(int rowIndex, int columnIndex, float curData)
    {
        _bars[rowIndex, columnIndex].transform.position = new Vector3(rowIndex, curData / 2.0f * 10.0f, columnIndex);
        _bars[rowIndex, columnIndex].transform.localScale = new Vector3(1, curData * 10.0f, 1);
        if (!_configuration.IsMirrored) return;

        _bars[MirrorRowIndex(rowIndex), columnIndex].transform.position = new Vector3(MirrorRowIndex(rowIndex), curData / 2.0f * 10.0f, columnIndex);
        _bars[MirrorRowIndex(rowIndex), columnIndex].transform.localScale = new Vector3(1, curData * 10.0f, 1);

    }

    private int BarGridWidth
    {
        get { return _configuration.IsMirrored ? _configuration.NumberOfBars * 2 : _configuration.NumberOfBars; }
    }

    private int BarGridLength
    {
        get { return _configuration.HistoryFrameCount; }
    }

    private int MirrorRowIndex(int rowIndex)
    {
        return _configuration.NumberOfBars *2 - rowIndex - 1;
    }
}