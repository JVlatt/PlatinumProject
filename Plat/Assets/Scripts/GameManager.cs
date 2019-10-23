using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Script
{
    public class GameManager
    {
        static GameManager _manger = null;

        #region Variables

        private PlayerManager m_players;
        public PlayerManager _players
        {
            get { return m_players; }
            set { m_players = value; }
        }

        private UIManager m_UIManager;
        public UIManager _UIManager
        {
            get { return m_UIManager; }
            set { m_UIManager = value; }
        }

        private LevelManager m_levelManager;
        public LevelManager _levelManager
        {
            get { return m_levelManager; }
            set { m_levelManager = value; }
        }

        private SoundManager m_soundManager;
        public SoundManager _soundManager
        {
            get { return m_soundManager; }
            set { m_soundManager = value; }
        }

        private InputManager m_inputManager;
        public InputManager _inputManager
        {
            get { return m_inputManager; }
            set { m_inputManager = value; }
        }
        private PeonManager m_peonManager;
        public PeonManager _peonManager
        {
            get { return m_peonManager; }
            set { m_peonManager = value; }
        }
        private TrainManager m_trainManager;
        public TrainManager _trainManager
        {
            get { return m_trainManager; }
            set { m_trainManager = value; }
        }

        private PhaseManager _phaseManager;
        public PhaseManager phaseManager
        {
            get { return _phaseManager; }
            set { _phaseManager = value; }
        }

        private CameraController _cameraController;
        public CameraController cameraController
        {
            get { return _cameraController; }
            set { _cameraController = value; }
        }
        #endregion

        public static GameManager GetManager()
        {
            if (_manger == null)
            {
                _manger = new GameManager(); // patern singleton
            }
            return _manger;
        }

    }
}
