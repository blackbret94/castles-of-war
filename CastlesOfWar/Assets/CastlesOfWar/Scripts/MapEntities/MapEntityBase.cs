using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Vashta.CastlesOfWar.Simulation;

namespace Vashta.CastlesOfWar.MapEntities
{
    public class MapEntityBase : MonoBehaviour, ISimulatedObject
    {
        [Tooltip("Instance name for debugging purposes only")]
        public string DebugName;
        public short TeamIndex { get; private set; }
        [FormerlySerializedAs("Collider")] public MapEntityCollider CollisionCollider;
        public MapEntityAuraCollider AuraCollider;
        public bool StartsConstructed = true;
        public short StartingTeamIndex = -1;
        public MapEntityData MapEntityData;
        public List<EntityCaptureVisual> CaptureVisuals;
        public float CaptureValue { get; private set; } // -MaxCapture for left, MaxCapture for right
        public GameManager GameManager { get; private set; }

        private void Awake()
        {
            if (CollisionCollider != null)
            {
                CollisionCollider.MapEntity = this;
            }

            if (AuraCollider != null)
            {
                AuraCollider.MapEntity = this;
            }

            // Set up team
            TeamIndex = StartingTeamIndex;
            if (StartingTeamIndex == 0)
                CaptureValue = -MapEntityData.CaptureTime;
            else if (StartingTeamIndex == 1)
                CaptureValue = MapEntityData.CaptureTime;
            
            // Init visuals
            float captureTime = MapEntityData.CaptureTime;
            float capturePerc = Mathf.Abs(CaptureValue / captureTime);
            foreach (EntityCaptureVisual captureVisual in CaptureVisuals)
            {
                captureVisual.UpdateValues(TeamIndex, capturePerc, CaptureValue);
            }
        }

        private void Start()
        {
            GameManager = GameManager.GetInstance();
        }
        
        public void OneStep(float timestep)
        {
            // Handle capture
            if (MapEntityData.CanCapture && CollisionCollider != null)
            {
                short dominatingTeamIndex = CollisionCollider.GetDominantTeam();
                if (dominatingTeamIndex != -1 && dominatingTeamIndex != TeamIndex)
                {
                    // Calculate
                    float captureTime = MapEntityData.CaptureTime;
                    float sign = dominatingTeamIndex == 0 ? -1 : 1;
                    float oldSign = CaptureValue > 0 ? 1 : -1;
                    CaptureValue += timestep * sign;
                    CaptureValue = Mathf.Clamp(CaptureValue, -captureTime, captureTime);
                    
                    // Check for sign change
                    float newSign = CaptureValue > 0 ? 1 : -1;
                    if (!Mathf.Approximately(newSign, oldSign))
                    {
                        // set to neutral
                        TeamIndex = -1;
                        OnNeutralized();
                    }
                    
                    // Check for team change
                    short newTeamIndex = TeamIndex;
                    if (Mathf.Approximately(CaptureValue, -captureTime))
                        newTeamIndex = 0;
                    else if (Mathf.Approximately(CaptureValue, captureTime))
                        newTeamIndex = 1;

                    if (newTeamIndex != TeamIndex)
                    {
                        // set to new team capture
                        TeamIndex = newTeamIndex;
                        OnCapture();
                    }
                    
                    // Update visuals
                    float capturePerc = Mathf.Abs(CaptureValue / captureTime);
                    foreach (EntityCaptureVisual captureVisual in CaptureVisuals)
                    {
                        captureVisual.UpdateValues(TeamIndex, capturePerc, CaptureValue);
                    }
                }
            }
        }

        protected virtual void OnNeutralized()
        {
            if (AuraCollider)
            {
                AuraCollider.ClearOverlaps();
            }
            
            GameManager.EventEntityChanged();
        }

        protected virtual void OnCapture()
        {
            if (AuraCollider)
            {
                AuraCollider.ReCheckForOverlaps();
            }
            
            GameManager.EventEntityChanged();
        }
    }
}