using JAFprocedural;
using UnityEngine;
using UnityEngine.UI;

public class GmapDisplay : MonoBehaviour
{
    public static GmapDisplay Instance;
    public Text words;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateMap(Space2D map)
    {
        words.text = "";
        for(int i = 0; i < map.height; words.text += "\n", i++)
        {
            for(int j = 0; j < map.width; j++)
            {
                if(map.GetCell(j, i) == 1000)
                {
                    words.text += "#";
                }else if(map.GetCell(j, i) == 1)
                {
                    words.text += " ";
                }
                else
                {
                    words.text += "?";
                }
            }
        }
    }
}
