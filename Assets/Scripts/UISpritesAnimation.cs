using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class UISpritesAnimation : MonoBehaviour
{
    public float velocidade;
    public bool isPlayer;

    [SerializeField] private Sprite[] player;
    [SerializeField] private Sprite[] mestre1;
    [SerializeField] private Sprite[] mestre2;
    [SerializeField] private Sprite[] mestre3;
    [SerializeField] private Sprite[] orochi;
    [SerializeField] private Sprite[] orochiTransformado;
    [SerializeField] private Sprite[] mendigo;
    [SerializeField] private Sprite[] caio;
    private Sprite[] sprites;

    private Image image;
    private int index = 0;
    private float timer = 0;

    void Start()
    {
        image = GetComponent<Image>();

        if (isPlayer)
        {
            sprites = player;
        } else {
            int inimigo = PlayerStatus.getProximaBatalha();

            if (inimigo < 3)
            {
                sprites = mestre1;
            } else if (inimigo < 6)
            {
                sprites = mestre2;
            }
            else if(inimigo < 9)
            {
                sprites = mestre3;
            }
            else if(inimigo < 11)
            {
                sprites = orochi;
            }
            else if(inimigo == 11)
            {
                sprites = mendigo;
            }
            else if(inimigo == 12)
            {
                sprites = orochiTransformado;
                velocidade *= 3;
                this.transform.localScale = new Vector3(0.75f, 0.75f, 1);
            }
            else
            {
                sprites = caio;
                this.transform.localScale = new Vector3(0.75f, 0.75f, 1);
            }
        }

        
    }
    private void Update()
    {
        if ((timer += Time.deltaTime) >= (velocidade))
        {
            timer = 0;
            image.sprite = sprites[index];
            index = (index + 1) % sprites.Length;
        }
    }
}
