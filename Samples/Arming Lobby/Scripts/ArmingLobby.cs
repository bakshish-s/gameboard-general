using System.Collections.Generic;
using UnityEngine;
namespace Hashbyte.GameboardGeneral
{
    public class ArmingLobby : MonoBehaviour, IPlayerUpdates
    {        
        public GameObject armedPlayerObj;
        [SerializeField] List<ArmingWidget> armingWidgets;
        [SerializeField] eArmingRestriction restrictions;
        [Range(1, 8)]
        [SerializeField] int maxPlayers;
        [SerializeField] TMPro.TextMeshProUGUI errorText;

        private Dictionary<ePlayerDirection, ArmingWidget> playersMap;
        private bool isInitialized;
        private List<ePlayerDirection> armedDirections;
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
                    playersMap.Add(armingWidget.direction, armingWidget);
                }
                else
                {
                    throw new System.Exception($"Player with direction {armingWidget.direction} already exists");
                }
            }
            HashbyteUserUpdates.Instance.Register(this);
            isInitialized = true;
            armedDirections = new List<ePlayerDirection>();
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
        public bool CanBeArmed(ePlayerDirection direction, bool isArming)
        {
            errorText.text = "";
            if (!isArming) { armedDirections.Remove(direction); CheckOtherPlayerDisplay(); return true; }
            else if (armedDirections.Count == 0) { armedDirections.Add(direction); return true; }            
            bool canBeArmed = true;
            if ((restrictions & eArmingRestriction.ONE_PER_SIDE) != 0) canBeArmed = RestrictToOnePlayerPerSide(direction, isArming) && canBeArmed;            
            if((restrictions & eArmingRestriction.OPPOSITE_ONLY) != 0) canBeArmed = RestrictOppositeOnly(direction, isArming) && canBeArmed;
            if (canBeArmed) armedDirections.Add(direction);
            CheckOtherPlayerDisplay();
            return canBeArmed;
        }

        private void CheckOtherPlayerDisplay()
        {
            if (armedDirections.Count == maxPlayers)
            {
                foreach (ePlayerDirection armingPosition in playersMap.Keys)
                {
                    if (armedDirections.Contains(armingPosition)) continue;
                    playersMap[armingPosition].HideArming(true);
                }
            }
            else
            {
                foreach (ePlayerDirection armingPosition in playersMap.Keys)
                {                    
                    playersMap[armingPosition].HideArming(false);
                }
            }
        }

        private bool RestrictToOnePlayerPerSide(ePlayerDirection playerDirection, bool isArming)
        {                        
            foreach(ePlayerDirection currentDirection in armedDirections)
            {
                 if (currentDirection.IsOnSameSide(playerDirection))
                 {
                    errorText.text = $"Players on the same side cannot be armed";
                    return false;
                 }
            }            
            return true;
        }        
        private bool RestrictOppositeOnly(ePlayerDirection playerDirection, bool isArming)
        {
            foreach (ePlayerDirection currentDirection in armedDirections)
            {                
                if (!currentDirection.IsOnOppositeSide(playerDirection))
                {
                    errorText.text = $"Only players on opposite side can play";
                    return false;
                }
            }
            return true;
        }
        public void PlayerUpdate(ePlayerDirection direction, PlayerPresence playerPresence)
        {

        }

        public void OnPlayerLoginToDrawer(ePlayerDirection direction, PlayerPresence presence)
        {
            if(!isInitialized) Initialize();
            //Debug.Log($"Player login to drawer received {direction} {presence.userName}");
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
