using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Minesweeper
{
    public class Point
    {
        public int i;
        public int j;

        public Point( int i, int j )
        {
            this.i = i;
            this.j = j;
        }

        public override string ToString()
        {
            return "(i " + i + ", j " + j + ")";
        }
    }

    public class BlockObject : MonoBehaviour
    {
        public int count;
        public bool hasVisited;

        public Image overlay;
        public Image mineImg;
        public Text countText;
        public Button parent;

        public Point point;

        private void Start()
        {
            parent = GetComponent<Button>();

            countText.gameObject.SetActive( false );
            parent.onClick.AddListener( () => OnButtonClick() );
        }

        public void Setup( int count, Point point, bool hasVisited = false )
        {
            if ( count == -1 )
                overlay.color = Color.green;

            this.count = count;
            this.hasVisited = hasVisited;
            this.point = point;

            SetCountText();
        }

        public void SetCountText()
        {
            countText.text = count == 0 ? string.Empty : count.ToString();
        }

        public void SetCounter()
        {
            if ( count == -1 )
                return;

            count++;
            countText.text = count == 0 ? string.Empty : count.ToString();
        }

        private void OnButtonClick()
        {
            //overlay.gameObject.SetActive( false );
            var color = overlay.color;
            overlay.color = new Color( color.r, color.g, color.b, 0.5f ); //.gameObject.SetActive( false );

            var gameController = FindObjectOfType<GameController>();

            if ( count == -1 )
            {
                mineImg.gameObject.SetActive( true );
                gameController.OnGameover();
            }
            else
            {
                hasVisited = true;
                countText.gameObject.SetActive( true );

                //gameController.OnBlockVisited();

                StartCoroutine( OpenBlock( gameController ) );
            }
        }

        private IEnumerator OpenBlock( GameController gameController, float delay = 0.25f )
        {
            if ( hasVisited == true || count == -1 )
                yield return null;

            hasVisited = true;
            gameController.OnBlockVisited();

            var color = overlay.color;
            overlay.color = new Color( color.r, color.g, color.b, 0.5f ); //.gameObject.SetActive( false );

            if ( count != -1 )
                countText.gameObject.SetActive( true );
            else
                mineImg.gameObject.SetActive( true );

            yield return new WaitForSeconds( delay );

            var blocks = gameController.GetObjectsByIndex( this );
            for ( var i = 0; i < blocks.Length; i++ )
            {
                if ( blocks[i] == null )
                    break;

                StartCoroutine( blocks[i].OpenBlock( gameController ) );
            }
        }
    }
}