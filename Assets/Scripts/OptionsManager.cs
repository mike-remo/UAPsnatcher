using UnityEngine;
using UnityEngine.UI;

public class OptionsManager : MonoBehaviour
{
    public static OptionsManager optionsManager;
    private AudioSource audio;
    private string dataFilename = "/savedata.json";
    private string dataFilepath = Application.dataPath;

    [System.Serializable] public class OptionsData
    {
        public string appName = "UAP_SNATCHER";
        public float volume;
    }
    public OptionsData options;

    public void SaveData()
    {
        Debug.Log("Saving Data to: " + dataFilepath + dataFilename);
        OptionsData saveOptions = new OptionsData();
        saveOptions.volume = options.volume;
        string dataJson = JsonUtility.ToJson(saveOptions);
        string savePath = dataFilepath + dataFilename;
        System.IO.File.WriteAllText(savePath, dataJson);
    }

    public void LoadData()
    {
        string loadPath = dataFilepath + dataFilename;
        if (System.IO.File.Exists(loadPath))
        {
            string dataJson = System.IO.File.ReadAllText(loadPath);
            OptionsData loadOptions = JsonUtility.FromJson<OptionsData>(dataJson);
            options.volume = loadOptions.volume;
        }
        else
        {
            options.volume = 1;
            Debug.Log("Could not load options data!");
        }
    }

    void Awake()
    {
        if (optionsManager != null)
        {
            Destroy(gameObject);
            return;
        }
        optionsManager = this;
        DontDestroyOnLoad(gameObject);
    }
    void Start()
    {
        LoadData();
        if (GameObject.Find("MenuAudio").TryGetComponent<AudioSource>(out audio))
            audio.volume = options.volume;
    }
}
