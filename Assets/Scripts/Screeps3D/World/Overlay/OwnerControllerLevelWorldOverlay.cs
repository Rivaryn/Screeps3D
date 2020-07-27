﻿using System;
using System.Collections.Generic;
using Assets.Scripts.Screeps3D;
using Assets.Scripts.Screeps3D.World.Views;
using Common;
using Screeps3D.Player;
using Screeps3D.Rooms;
using UnityEngine;

namespace Screeps3D.World.Views
{
    // We want an overlay manager of sorts that can initialized room specific overlay and toggle them on / off
    // we need to initialize worldoverlay views for each room,

    public class OwnerControllerLevelWorldOverlay : MonoBehaviour
    {
        private Dictionary<string, WorldView> _views = new Dictionary<string, WorldView>();

        // TODO: we will initialize a list of OwnerControllerLevelViewData
        public OwnerControllerLevelWorldOverlay()
        {

            // TODO: we want to initialize 1 view, this view is then responsible for world wide rendering
            // inside this view, we might want object pooling, to render "sub views" over each room, in this case an OwnerControllerLevelPrefab for each room
            // we should be able to control how many of theese sub views we render, for example based on distance from player.
            // other kind of world views would be some sort of strategic view, intel, map visuals and such.


            // TODO: for each view, or well actually inside the view
            //_label.text = string.Format("{0}", _selected.Owner.Username);
            //_badge.sprite = Sprite.Create(_selected.Owner.Badge,
            //    new Rect(0.0f, 0.0f, BadgeManager.BADGE_SIZE, BadgeManager.BADGE_SIZE), new Vector2(.5f, .5f));
        }

        private void Awake()
        {
            MapStatsUpdater.Instance.OnMapStatsUpdated += Instance_OnMapStatsUpdated;
        }

        private void Instance_OnMapStatsUpdated()
        {
            // TODO: scan X rooms in direction outward from player
            if (MapStatsUpdater.Instance.RoomInfo.TryGetValue(PlayerPosition.Instance.ShardName, out var shardRoomInfo))
            {
                foreach (var roomInfo in shardRoomInfo)
                {
                    if (!_views.TryGetValue(roomInfo.RoomName, out var view))
                    {
                        Scheduler.Instance.Add(() =>
                        {
                            var room = RoomManager.Instance.Get(roomInfo.RoomName, PlayerPosition.Instance.ShardName);
                            var data = new OwnerControllerLevelData(room, roomInfo);
                            var o = WorldViewFactory.GetInstance(data);
                            o.name = $"{roomInfo.RoomName}:RoomOwnerInfoView";
                            _views[roomInfo.RoomName] = o;
                        });
                    }

                    // TODO: trigger an update?
                }
            }

        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.KeypadPlus))
            {
                OwnerControllerLevelView.overlayCameraAngleThreshold += 0.1f;
                Debug.Log($"overlayCameraAngleThreshold: {OwnerControllerLevelView.overlayCameraAngleThreshold}");
            }
            else if (Input.GetKeyDown(KeyCode.KeypadMinus))
            {
                OwnerControllerLevelView.overlayCameraAngleThreshold -= 0.1f;
                Debug.Log($"overlayCameraAngleThreshold: {OwnerControllerLevelView.overlayCameraAngleThreshold}");
            }

            if (Input.GetKeyUp(KeyCode.Keypad9))
            {
                OwnerControllerLevelView.overlayCameraHeightThreshold -= 10f;
                Debug.Log($"overlayCameraHeightThreshold: {OwnerControllerLevelView.overlayCameraHeightThreshold}");
            }
            else if (Input.GetKeyUp(KeyCode.Keypad6))
            {
                OwnerControllerLevelView.overlayCameraHeightThreshold += 10f;
                Debug.Log($"overlayCameraHeightThreshold: {OwnerControllerLevelView.overlayCameraHeightThreshold}");
            }

        }

    }

    public class OwnerControllerLevelData : WorldViewData
    {
        public OwnerControllerLevelData(Room room, RoomInfo roomInfo)
        {
            Type = "RoomOwnerInfo";
            Room = room;
            RoomInfo = roomInfo;
        }

        public Room Room { get; }
        public RoomInfo RoomInfo { get; }
    }
}