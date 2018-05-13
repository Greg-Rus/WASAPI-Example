using UnityEngine;

public class Main : MonoBehaviour
{
    public SoundCapture SoundCapture;
    public BasicSingleFrameModel BasicSingleFrameModel;
    public BarGraphController BarGraphController;
    public MultiBarGraphController MultiBarGraphController;
    public BarFactory BarFactory;

    void Awake()
    {
        BasicSingleFrameModel.Inject(SoundCapture);
        BasicSingleFrameModel.Setup();

        var multiFrameModel = new FrameModel2D(BasicSingleFrameModel);
        MultiBarGraphController = new MultiBarGraphController(multiFrameModel, BarFactory);
    }
}