using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UniRx;
public sealed class ImageMover : MonoBehaviour
{    
    [SerializeField] private Button ResetButton;    
    [SerializeField] private Slider Speed;        
    public static ImageMover instance;    
    private CompositeDisposable disposables = new CompositeDisposable();    
    private Touch Touch;
    private const string speedKey = "speed";
    private void Awake()
    {        
        #region singleton
        if (instance == null)
            instance=this;
        else 
            Destroy(gameObject);
#endregion
        Init();
    }

    public void Init()
    {
        transform.position = Loader.LoadPos();
        Speed.value = PlayerPrefs.GetFloat(speedKey);
        ResetButton.onClick.AddListener(Reset);
#if UNITY_ANDROID
        var ClickStream = Observable.EveryUpdate().
            Where(_ => Touch.phase == TouchPhase.Ended).
            ObserveOnMainThread().Subscribe(_ =>
            {
                if (!EventSystem.current.IsPointerOverGameObject())
                {
                    ICommand command = new PointToMove(Touch.position);
                    CommandInvoker.AddCommand(command);
                }
            });        
        disposables.Add(ClickStream);
#elif UNITY_EDITOR
        var ClickStream = Observable.EveryUpdate().
            Where(_ => Input.GetMouseButtonUp(0)).
            ObserveOnMainThread().Subscribe(_ =>
            {
                if (!EventSystem.current.IsPointerOverGameObject())
                {                   
                    ICommand command = new PointToMove(Input.mousePosition);
                    CommandInvoker.AddCommand(command);
                }
            });
        disposables.Add(ClickStream);
#endif
    }
    
    public void Move(Vector2 destination)
    {        
        if(CommandInvoker.commandBuffer.Count>0)
            transform.position = destination == null ? transform.position : Vector3.Lerp(transform.position, destination, this.Speed.value * Time.deltaTime);
    }    

    public void Reset()
    {
        CommandInvoker.ClearCommand();       
        transform.position = transform.position;        
    }
    
    private void OnApplicationQuit()
    {
        PlayerPrefs.SetFloat(speedKey, Speed.value);
        Loader.SavePos(transform.position);
        Loader.SaveCommands(CommandInvoker.commandBuffer);
        disposables.Dispose();
    }
}