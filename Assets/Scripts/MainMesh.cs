using UnityEngine;

public class MainMesh : MonoBehaviour
{
    public SoundCapture SoundCapture;
    public BasicSingleFrameModel BasicSingleFrameModel;

    public MeshConfig Config;

    void Awake()
    {
        BasicSingleFrameModel.Inject(SoundCapture);
        BasicSingleFrameModel.Setup();
        var meshModel = new MeshModel(BasicSingleFrameModel, Config);
    }
}