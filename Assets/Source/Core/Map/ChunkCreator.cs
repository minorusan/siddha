using UnityEngine;
using System.Collections;
using Utils;
using System.Collections.Generic;
using System.Linq;
using Core.Characters.Player;


namespace Core.Map
{
    public class ChunkCreator : MonoBehaviour
    {
        #region Private

        private const string kPathToChunkRoomPrefabs = "Prefabs/Decorations/Chunks/{0}/Rooms/";
        private const string kPathToChunkEndRoomPrefabs = "Prefabs/Decorations/Chunks/{0}/Rooms/End/";
        private const string kPathToStartRoomPrefabs = "Prefabs/Decorations/Chunks/{0}/Rooms/Start/";
        private Room[] _rooms;
        private Room[] _endRooms;
        private Room[] _startRooms;
        private List<Room> _generatedRoomsList = new List<Room>();

        #endregion

        public int MaxRoomsCouns;
        public string ChunkName;

#if UNITY_EDITOR
        [ReadOnly]
#endif
        public Room[] GeneratedRooms;

        private void OnEnable()
        {
            Camera.main.GetComponent<CameraFollow>().enabled = false;
            _rooms = Resources.LoadAll<Room>(string.Format(kPathToChunkRoomPrefabs, ChunkName));
            _endRooms = Resources.LoadAll<Room>(string.Format(kPathToChunkEndRoomPrefabs, ChunkName));
            _startRooms = Resources.LoadAll<Room>(string.Format(kPathToStartRoomPrefabs, ChunkName));
            GenerateChunk();
        }

        private void OnDisable()
        {
            _generatedRoomsList.Clear();
            for (int i = 0; i < transform.childCount; i++)
            {
                Destroy(transform.GetChild(i).gameObject);
            }
        }

        private void GenerateChunk()
        {
            var firstRoom = Instantiate(_startRooms[Random.Range(0, _startRooms.Length)]);
            firstRoom.transform.parent = transform;
            firstRoom.transform.localPosition = Vector3.zero;

            firstRoom.gameObject.SetActive(true);
            firstRoom.Map.InstantiateCells();

            _generatedRoomsList.Add(firstRoom);

            for (int i = 0; i < firstRoom.Exits.Length; i++)
            {
                if (_generatedRoomsList.Count <= MaxRoomsCouns)
                {
                    GenerateRoomForAnExitOfRoom(firstRoom.Exits[i], firstRoom.gameObject);
                }
            }

            SetLastRoom();
            PlayerBehaviour.SetPlayerPosition(_generatedRoomsList[0].Map.CenterNode.Position);

            foreach (var item in _generatedRoomsList)
            {
                item.Map.InstantiateCells();
            }
            Camera.main.GetComponent<CameraFollow>().enabled = true;
        }

        private void GenerateRoomForAnExitOfRoom(RoomExit exit, GameObject owner)
        {
            var prefabsList = new List<Room>(_rooms);
            var ownerName = owner.name.Replace("(Clone)", "");
            var roomsThatFit = prefabsList.Where(r => r.Exits.Any(e => e.ExitSide == exit.LinksWithSide
                              ) && !r.name.Contains(ownerName)).ToList();

            if (roomsThatFit.Count < 1)
            {
                return;
            }

            var newNeighbour = roomsThatFit[Random.Range(0, roomsThatFit.Count)];

            var instantiatedNeighbour = Instantiate(newNeighbour);
            _generatedRoomsList.Add(instantiatedNeighbour);
            instantiatedNeighbour.transform.parent = transform;
            instantiatedNeighbour.transform.localPosition = Vector3.zero;

            instantiatedNeighbour.gameObject.SetActive(true);
            instantiatedNeighbour.Map.InstantiateCells();

            PairExitWithRoom(exit, instantiatedNeighbour);

            var unCheckedExits = instantiatedNeighbour.Exits.Where(e => e.LinkedWith == null).ToArray();
            for (int i = 0; i < unCheckedExits.Length; i++)
            {
                if (_generatedRoomsList.Count < MaxRoomsCouns)
                {
                    GenerateRoomForAnExitOfRoom(unCheckedExits[i], instantiatedNeighbour.gameObject);
                }
            }
        }

        private void PairExitWithRoom(RoomExit exit, Room linkedRoom)
        {
            var roomExits = linkedRoom.Exits;
            var exitThatFit = roomExits.FirstOrDefault(e => e.ExitSide == exit.LinksWithSide && e.LinkedWith == null);
            if (exitThatFit)
            {
                exitThatFit.LinkedWith = exit;
                exit.LinkedWith = exitThatFit;

                var yDifference = exit.transform.position.y - exitThatFit.transform.position.y;
                var xDifference = exit.transform.position.x - exitThatFit.transform.position.x;
                var adjustmentVector = new Vector3(linkedRoom.transform.position.x + xDifference,
                                                    linkedRoom.transform.position.y + yDifference, 0);
                linkedRoom.transform.position = adjustmentVector;
            }
        }

        private void SetLastRoom()
        {
            var lastRoom = _generatedRoomsList.Last();

            var prefabsList = new List<Room>(_endRooms);
            var exit = lastRoom.Exits.FirstOrDefault(e => e.LinkedWith == null);
            if (exit != null)
            {
                var roomThatFit = prefabsList.First(r => r.Exits.Any(e => e.ExitSide == exit.LinksWithSide));
                if (roomThatFit != null)
                {
                    var instantiatedNeighbour = Instantiate(roomThatFit);
                    _generatedRoomsList.Add(instantiatedNeighbour);
                    instantiatedNeighbour.transform.parent = transform;
                    instantiatedNeighbour.transform.localPosition = Vector3.zero;

                    instantiatedNeighbour.gameObject.SetActive(true);
                    instantiatedNeighbour.Map.InstantiateCells();
                    PairExitWithRoom(exit, roomThatFit);
                }
            }
        }


    }
}

