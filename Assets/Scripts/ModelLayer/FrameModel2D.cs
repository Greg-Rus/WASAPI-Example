using System.Runtime.Remoting.Messaging;
using UniRx;
using UnityEngine.Rendering;

public class FrameModel2D
{
    private readonly IReactiveBarData _singleFrameModel;
    private readonly Configuration _config;
    private ReactiveProperty<float>[,] _multiFrameReactiveProperties;


    public FrameModel2D(IReactiveBarData singleFrameModel, Configuration config)
    {
        _singleFrameModel = singleFrameModel;
        _config = config;
        SetupReactiveArrays();
        SetupPropagationSubscriptions();
        SetupSingleFrameSubscriptions();
    }

    private void SetupReactiveArrays()
    {
        _multiFrameReactiveProperties = new ReactiveProperty<float>[_config.NumberOfBars, _config.HistoryFrameCount];

        for (int i = 0; i < _config.NumberOfBars; i++)
        {
            for (int j = 0; j < _config.HistoryFrameCount; j++)
            {
                _multiFrameReactiveProperties[i,j] = new ReactiveProperty<float>(0);
            }
        }
    }

    private void SetupSingleFrameSubscriptions()
    {
        for (int i = 0; i < _config.NumberOfBars; i++)
        {
            var rowIndex = i;
            _singleFrameModel.GetReactiveBarAtIndex(i)
                .Subscribe(value => UpdateRow(rowIndex, value));
        }
    }

    private void UpdateRow(int rowIndex, float value)
    {
        _multiFrameReactiveProperties[rowIndex, 0].Value = value;
    }

    private void SetupPropagationSubscriptions()
    {
        for (int i = 0; i < _config.NumberOfBars; i++)
        {
            for (int j = 1; j < _config.HistoryFrameCount; j++)
            {
                var rowIndex = i;
                var columnIndex = j;
                _multiFrameReactiveProperties[i, j - 1].Pairwise((oldValue, newVal) => oldValue)
                    .Subscribe(oldValue => UpdateBarAtPosition(rowIndex, columnIndex, oldValue));
            }
        }
    }

    private void UpdateBarAtPosition(int rowIndex, int columnIndex, float oldValue)
    {
        _multiFrameReactiveProperties[rowIndex, columnIndex].Value = oldValue;
    }

    public ReactiveProperty<float> GetReactiveBarAtIndex(int row, int column)
    {
        return _multiFrameReactiveProperties[row, column];
    }
}