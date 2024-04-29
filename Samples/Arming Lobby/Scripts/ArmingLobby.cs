using System.Collections.Generic;
using UnityEngine;
namespace Hashbyte.GameboardGeneral
{
    public class ArmingLobby : MonoBehaviour, IPlayerUpdates
    {        
        [SerializeField] List<ArmingWidget> armingWidgets;
        public GameObject armedPlayerObj;
        public eArmingRestriction restrictions;
        private Dictionary<ePlayerDirection, ArmingWidget> playersMap;
        private bool isInitialized;
        #region Singleton
        public static ArmingLobby Instance { get; private set; }
        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            Initialize();
        }
        #endregion

        #region Initialization
        private void Initialize()
        {
            playersMap = new Dictionary<ePlayerDirection, ArmingWidget>();            
            foreach(ArmingWidget armingWidget in armingWidgets)
            {
                if (!playersMap.ContainsKey(armingWidget.direction))
                {
                    Debug.Log($"Drawer added at position {armingWidget.direction}");
                    playersMap.Add(armingWidget.direction, armingWidget);
                }
                else
                {
                    throw new System.Exception($"Player with direction {armingWidget.direction} already exists");
                }
            }
            HashbyteUserUpdates.Instance.Register(this);
            isInitialized = true;
#if UNITY_EDITOR
            editorTest = new EditorTest();
#endif
        }
        #endregion

#if UNITY_EDITOR
        EditorTest editorTest;
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                 for(int i=0; i<8; i++)
                {
                    OnPlayerLoginToDrawer(editorTest.GetUser(out PlayerPresence presence), presence);
                }
            }
        }
#endif
        public bool CanBeArmed(ePlayerDirection direction, bool isArmed)
        {
            if ((restrictions & eArmingRestriction.ONE_PER_SIDE) != 0) return RestrictToOnePlayerPerSide();
            else if ((restrictions & eArmingRestriction.TWO_PLAYER) != 0) return RestrictToTwoPlayers();
            else if((restrictions & eArmingRestriction.OPPOSITE_ONLY) != 0) return RestrictOppositeOnly();
            return true;
        }

        private bool RestrictToOnePlayerPerSide()
        {
            return true;
        }
        private bool RestrictToTwoPlayers()
        {
            return true;
        }
        private bool RestrictOppositeOnly()
        {
            return true;
        }
        public void PlayerUpdate(ePlayerDirection direction, PlayerPresence playerPresence)
        {

        }

        public void OnPlayerLoginToDrawer(ePlayerDirection direction, PlayerPresence presence)
        {
            if(!isInitialized) Initialize();
            Debug.Log($"Player login to drawer received {direction} {presence.userName}");
            playersMap[direction].UpdatePresence(presence);
        }

        public void OnPlayerLogout(ePlayerDirection direction, PlayerPresence presence)
        {
            if (!isInitialized) Initialize();
            Debug.Log($"Player logout to drawer received {direction} {presence.userName}");
            playersMap[direction].UpdatePresence(presence);
        }

        public void OnPlayerChangePosition(ePlayerDirection direction, PlayerPresence presence)
        {
            if (!isInitialized) Initialize();
            Debug.Log($"Player position change received {direction}, {presence.userName}");
            playersMap[direction].UpdatePresence(presence);
        }

        public void PlayerDisarmedBySystem(ePlayerDirection direction, PlayerPresence presence)
        {
            
        }
    }
}
