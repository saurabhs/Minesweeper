using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Game.Minesweeper
{
    public class Menu : MonoBehaviour
    {
        public Button playButton;
        public Slider gridSizeSlider;
        public Slider mineCountSlider;

        private int maxMinesPossible;

        private void Start()
        {
            AddListeners();
            SetupGridSizeScrollbar();
            SetupMinesCountScrollbar();
        }

        private void AddListeners()
        {
            playButton.onClick.AddListener( () => OnPlayButtonClick() );
        }

        private void SetupGridSizeScrollbar()
        {
            gridSizeSlider.onValueChanged.AddListener( delegate
            { OnGridSizeScrollbarValueChange( gridSizeSlider ); } );
        }

        private void SetupMinesCountScrollbar()
        {
            mineCountSlider.onValueChanged.AddListener( delegate
            { OnMineCountScrollbarValueChange( mineCountSlider ); } );
        }

        private void OnGridSizeScrollbarValueChange( Slider slider )
        {
            PlayerPrefs.SetInt( "GridSize", ( int )slider.value );

            //maxMinesPossible = ( int )gridSizeSlider.value / 10;
            //mineCountSlider.maxValue = ( int )gridSizeSlider.value / 10;
        }

        private void OnMineCountScrollbarValueChange( Slider slider )
        {
            //print( mineCountSlider.value );
            PlayerPrefs.SetInt( "MineCount", ( int )slider.value );
        }

        private void OnPlayButtonClick()
        {
            PlayerPrefs.Save();
            SceneManager.LoadScene( "Game" );
        }
    }
}