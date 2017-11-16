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

        public Text gridSizeSliderHandleCounterText;
        public Text mineCountSliderHandleCounterText;

        public Text gameOverText;

        private void Start()
        {
            SetupInputs();
            SetupInitialValuesForSliders();
            SetupGameOverText();
        }

        private void SetupInputs()
        {
            playButton.onClick.AddListener( () => OnPlayButtonClick() );

            gridSizeSlider.onValueChanged.AddListener( delegate
            { OnGridSizeScrollbarValueChange( gridSizeSlider ); } );

            mineCountSlider.onValueChanged.AddListener( delegate
            { OnMineCountScrollbarValueChange( mineCountSlider ); } );
        }

        private void SetupInitialValuesForSliders()
        {
            var gridSize = PlayerPrefs.GetInt( "GridSize" );
            gridSizeSlider.value = gridSize != 0 ? gridSize : gridSizeSlider.minValue;

            var mineCount = PlayerPrefs.GetInt( "MineCount" );
            mineCountSlider.value = mineCount != 0 ? mineCount : mineCountSlider.minValue;

            gridSizeSliderHandleCounterText.text = (( int )gridSizeSlider.value).ToString();
            mineCountSliderHandleCounterText.text = (( int )mineCountSlider.value).ToString();
        }

        private void SetupGameOverText()
        {
            var gameoverState = PlayerPrefs.GetString( "Cleared" );
            gameOverText.text = gameoverState.Equals( string.Empty ) ? string.Empty : gameoverState.Equals( "Cleared" ) ? "YAY!!1!1" : "HUEHUEHUE";
        }

        #region sliders events

        private void OnGridSizeScrollbarValueChange( Slider slider )
        {
            gridSizeSliderHandleCounterText.text = (( int )slider.value).ToString();
            mineCountSlider.maxValue = slider.value;
        }

        private void OnMineCountScrollbarValueChange( Slider slider )
        {
            mineCountSliderHandleCounterText.text = (( int )slider.value).ToString();
        }

        #endregion

        #region button events

        private void OnPlayButtonClick()
        {
            PlayerPrefs.SetInt( "GridSize", ( int )gridSizeSlider.value );
            PlayerPrefs.SetInt( "MineCount", ( int )mineCountSlider.value );

            PlayerPrefs.Save();
            SceneManager.LoadScene( "Game" );
        }

        #endregion

        #region OnQuit

        private void OnApplicationQuit()
        {
            PlayerPrefs.SetString( "Cleared", string.Empty );
        }

        #endregion
    }
}