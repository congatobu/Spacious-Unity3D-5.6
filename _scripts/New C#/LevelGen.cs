using UnityEngine;

public class LevelGen : MonoBehaviour
{
	public int mapSize = 20;
	public int[,] map;
	public Vector2 mapRatio = new Vector2(1, 1);
	public Rect newRoom;
	public Rect[] rooms;
	public int[] roomsLinked;
	public int sizeX;
	public int sizeY;
	public GameObject tilePrefab;
	public Sprite[] tileSprites;

	private void Awake()
	{
		//if(PlayerPrefsX.GetBool("saved"))
		//	LoadLevel();
		//else
		StartGen();
	}

	private void StartGen()
	{
		sizeX = (int) Mathf.Floor(mapSize*mapRatio.x);
		sizeY = (int) Mathf.Floor(mapSize*mapRatio.y);

		//set every tile in the map as 0
		map = new int[sizeX, sizeY];

		for (var x = 0; x < sizeX; x++)
		{
			for (var y = 0; y < sizeY; y++)
			{
				map[x, y] = 1;
			}
		}


		CreateRooms();
		//SquashRooms();

		FillRooms();

		MakeCorridors();
		//MakeCorridors();

		MakeWalls();

		CheckWalls();

		SaveLevel();

		MakeSprites();
	}

	private void CreateRooms()
	{
		//room creation
		int minSize = 3;
		int maxSize = 7;
		int roomCount = Random.Range(8, 12);
		rooms = new Rect[roomCount];

		//make rooms
		int i = 0;
		//Rect newRoom;
		while (i < roomCount)
		{
			newRoom.width = Mathf.Floor(Random.Range(minSize, maxSize));
			newRoom.height = Mathf.Floor(Random.Range(2, 4));

			if (i == 0)
			{
				newRoom.x = Mathf.Floor(sizeX/2 - newRoom.width/2);
				newRoom.y = sizeY - newRoom.height - 3;
			}
			else
			{
				newRoom.x = Mathf.Floor(Random.Range(3, sizeX - newRoom.width - 3));
				newRoom.y = Mathf.Floor(Random.Range(3, sizeY - newRoom.height - 3));
			}

			if (!DoesCollide(newRoom, -1))
			{
				rooms[i] = newRoom;
				i++;
			}
		}
	}

	private void FillRooms()
	{
		Debug.Log("filling rooms");
		for (int i = 0; i < rooms.Length; i++)
		{
			var room = rooms[i];
			for (int x = (int)room.x; x <= room.width + room.x; x++)
			{
				for (int y = (int)room.y; y <= room.height + room.y; y++)
				{
					map[x, y] = 0;
				}
			}
		}
	}

	private void MakeCorridors()
	{
		roomsLinked = new int[rooms.Length];

		for (int i = 0; i < rooms.Length; i++)
		{
			var roomA = rooms[i];
			var roomB = rooms[FindRoom(i)];

			var pointA = new Vector2(Mathf.Floor(Random.Range(roomA.x, roomA.x + roomA.width)),
				Mathf.Floor(Random.Range(roomA.y, roomA.y + roomA.height)));
			var pointB = new Vector2(Mathf.Floor(Random.Range(roomB.x, roomB.x + roomB.width)),
				Mathf.Floor(Random.Range(roomB.y, roomB.y + roomB.height)));

			//Vector2 pointA = roomA.center;
			//Vector2 pointB = roomB.center;

			while (pointA.x != pointB.x || pointA.y != pointB.y)
			{
				if (pointA.x != pointB.x)
				{
					if (pointB.x < pointA.x)
						pointB.x++;
					else
						pointB.x--;
				}
				else
				{
					if (pointA.y != pointB.y)
					{
						if (pointB.y < pointA.y)
							pointB.y++;
						else
							pointB.y--;
					}
				}

				map[(int)pointB.x, (int)pointB.y] = 0;
				//map[pointB.x,pointB.y+1] = 0;
			}
		}
	}

	private int FindRoom(int myId)
	{
		var newRoom = 0;
		var canLink = false;

		while (!canLink)
		{
			newRoom = (int) Mathf.Floor(Random.Range(0, rooms.Length - 1));
			foreach (var linkedRoom in roomsLinked)
			{
				canLink = true;
				if (linkedRoom == newRoom)
					canLink = false;
			}

			if (newRoom == myId)
				canLink = false;
		}

		roomsLinked[myId] = newRoom;

		return newRoom;
	}

	private bool DoesCollide(Rect room, int ignore)
	{
		for (var i = 0; i < rooms.Length; i++)
		{
			var check = rooms[i];

			if (i == ignore)
				continue;

			if (
				!((room.x + room.width < check.x) || (room.x > check.x + check.width + 2) || (room.y + room.height < check.y) ||
				  (room.y > check.y + check.height + 2)))
				return true;
		}

		return false;
	}

	private void SquashRooms()
	{
		for (var i = 0; i < 5; i++)
		{
			for (var j = 0; j < rooms.Length; j++)
			{
				var room = rooms[j];

				while (true)
				{
					var old_position = new Vector2(room.x, room.y);

					if (room.x > 1)
						room.x--;
					if (room.y + room.height < sizeY - 1)
						room.y++;
					if ((room.x == 1) && (room.y + room.height == sizeX - 1))
						break;

					if (DoesCollide(room, j))
					{
						room.x = old_position.x;
						room.y = old_position.y;
						break;
					}
				}
			}
		}
	}

	private void MakeWalls()
	{
		for (var x = 0; x < sizeX; x++)
		{
			for (var y = 0; y < sizeY; y++)
			{
				if (map[x, y] == 0)
				{
					for (var xx = x - 1; xx <= x + 1; xx++)
					{
						for (var yy = y - 1; yy <= y + 1; yy++)
						{
							if (map[xx, yy] == 1)
								map[xx, yy] = 2;
						}
					}
				}
			}
		}
	}

	private void CheckWalls()
	{
		for (var x = 0; x < sizeX; x++)
		{
			for (var y = 0; y < sizeY; y++)
			{
				if (map[x, y] == 2)
				{
					var around = new int[8];
					var count = 0;

					for (var yy = y + 1; yy >= y - 1; yy--)
					{
						for (var xx = x - 1; xx <= x + 1; xx++)
						{
							if (xx == x && yy == y)
								continue;

							around[count] = map[xx, yy];
							count++;
						}
					}

					map[x, y] = DefineSprites(around);
				}
			}
		}
	}

	private int DefineSprites(int[] around)
	{
		var newTile = 0;

		for (var i = 0; i < around.Length; i++)
		{
			if (around[i] > 0)
			{
				Debug.Log(around[i]);
				around[i] = 1;
				Debug.Log(around[i]);
			}
			else
			{
				Debug.Log(around[i]);
			}
		}

		Debug.Log(around.Length);

		if (around[0] == 0 && around[1] == 0 && around[2] == 0 && around[3] == 0 && 
			around[4] == 1 && around[5] == 0 && around[6] == 1 && around[7] == 1 ||
		around[0] == 0 && around[1] == 0 && around[2] == 0 && around[3] == 0 && 
			around[4] == 1 && around[5] == 1 && around[6] == 1 && around[7] == 1)
		{
			newTile = 2;
			Debug.Log("newTile = 2");
		}
		else if (around[0] == 0 && around[1] == 0 && around[2] == 0 && around[3] == 1 &&
			around[4] == 1 && around[5] == 1 && around[6] == 1 && around[7] == 1 ||
		around[0] == 1 && around[1] == 0 && around[2] == 0 && around[3] == 1 &&
			around[4] == 1 && around[5] == 1 && around[6] == 1 && around[7] == 1 ||
		around[0] == 0 && around[1] == 0 && around[2] == 1 && around[3] == 1 &&
			around[4] == 1 && around[5] == 1 && around[6] == 1 && around[7] == 1 ||
		around[0] == 1 && around[1] == 0 && around[2] == 1 && around[3] == 1 &&
			around[4] == 1 && around[5] == 1 && around[6] == 1 && around[7] == 1)
		{
			newTile = 3;
			Debug.Log("newTile = 3");
		}
		else if (around[0] == 0 && around[1] == 0 && around[2] == 0 && around[3] == 1 &&
			around[4] == 0 && around[5] == 1 && around[6] == 1 && around[7] == 0 ||
		around[0] == 0 && around[1] == 0 && around[2] == 0 && around[3] == 1 &&
			around[4] == 0 && around[5] == 1 && around[6] == 1 && around[7] == 1)
		{
			newTile = 4;
			Debug.Log("newTile = 4");
		}
		else if (around[0] == 0 && around[1] == 1 && around[2] == 1 && around[3] == 0 &&
			around[4] == 1 && around[5] == 0 && around[6] == 1 && around[7] == 1 ||
		around[0] == 1 && around[1] == 1 && around[2] == 1 && around[3] == 0 &&
			around[4] == 1 && around[5] == 0 && around[6] == 1 && around[7] == 1 ||
		around[0] == 0 && around[1] == 1 && around[2] == 1 && around[3] == 0 &&
			around[4] == 1 && around[5] == 1 && around[6] == 1 && around[7] == 1 ||
		around[0] == 1 && around[1] == 1 && around[2] == 1 && around[3] == 0 &&
			around[4] == 1 && around[5] == 1 && around[6] == 1 && around[7] == 1)
		{
			newTile = 5;
			Debug.Log("newTile = 5");
		}
		else if (around[0] == 1 && around[1] == 1 && around[2] == 0 && around[3] == 1 &&
			around[4] == 0 && around[5] == 1 && around[6] == 1 && around[7] == 0)
		{
			newTile = 6;
			Debug.Log("newTile = 6");
		}
		else if (around[0] == 0 && around[1] == 1 && around[2] == 1 && around[3] == 0 &&
			around[4] == 1 && around[5] == 0 && around[6] == 0 && around[7] == 0)
		{
			newTile = 7;
			Debug.Log("newTile = 7");
		}
		else if (around[0] == 1 && around[1] == 1 && around[2] == 1 && around[3] == 1 &&
			around[4] == 1 && around[5] == 0 && around[6] == 0 && around[7] == 0 ||

		around[0] == 1 && around[1] == 1 && around[2] == 1 && around[3] == 1 &&
			around[4] == 1 && around[5] == 1 && around[6] == 0 && around[7] == 0 ||

		around[0] == 1 && around[1] == 1 && around[2] == 1 && around[3] == 1 &&
			around[4] == 1 && around[5] == 0 && around[6] == 0 && around[7] == 1 ||

		around[0] == 1 && around[1] == 1 && around[2] == 1 && around[3] == 1 &&
			around[4] == 1 && around[5] == 1 && around[6] == 0 && around[7] == 1)
		{
			newTile = 8;
			Debug.Log("newTile = 8");
		}
		else if (around[0] == 1 && around[1] == 1 && around[2] == 0 && around[3] == 1 &&
			around[4] == 0 && around[5] == 0 && around[6] == 0 && around[7] == 0)
		{
			newTile = 9;
			Debug.Log("newTile = 9");
		}
		else if (around[0] == 0 && around[1] == 0 && around[2] == 0 && around[3] == 0 &&
			around[4] == 1 && around[5] == 0 && around[6] == 0 && around[7] == 0)
		{
			newTile = 10;
			Debug.Log("newTile = 10");
		}
		else if (around[0] == 0 && around[1] == 0 && around[2] == 0 && around[3] == 1 &&
			around[4] == 1 && around[5] == 0 && around[6] == 0 && around[7] == 0)
		{
			newTile = 11;
			Debug.Log("newTile = 11");
		}
		else if (around[0] == 0 && around[1] == 0 && around[2] == 0 && around[3] == 1 &&
			around[4] == 0 && around[5] == 0 && around[6] == 0 && around[7] == 0)
		{
			newTile = 12;
			Debug.Log("newTile = 12");
		}
		else if (around[0] == 1 && around[1] == 1 && around[2] == 1 && around[3] == 1 &&
			around[4] == 1 && around[5] == 0 && around[6] == 1 && around[7] == 1)
		{
			newTile = 16;
			Debug.Log("newTile = 16");
		}
		else if (around[0] == 1 && around[1] == 1 && around[2] == 1 && around[3] == 1 &&
			around[4] == 1 && around[5] == 1 && around[6] == 1 && around[7] == 0)
		{
			newTile = 17;
			Debug.Log("newTile = 17");
		}
		else if (around[0] == 0 && around[1] == 1 && around[2] == 1 && around[3] == 1 &&
			around[4] == 1 && around[5] == 1 && around[6] == 1 && around[7] == 1)
		{
			newTile = 18;
			Debug.Log("newTile = 18");
		}
		else if (around[0] == 1 && around[1] == 1 && around[2] == 0 && around[3] == 1 &&
			around[4] == 1 && around[5] == 1 && around[6] == 1 && around[7] == 1)
		{
			newTile = 19;
			Debug.Log("newTile = 19");
		}
		else
		{
			newTile = 1;
		}

		/*
		switch (around)
		{
			case around == [0, 0, 0, 0, 1, 0, 1, 1] || around == [0, 0, 0, 0, 1, 1, 1, 1]:
				newTile = 2;
				break;

			case around == [0, 0, 0, 1, 1, 1, 1, 1] || around == [1, 0, 0, 1, 1, 1, 1, 1] || around == [0, 0, 1, 1, 1, 1, 1, 1] || around == [1, 0, 1, 1, 1, 1, 1, 1]:
				newTile = 3;
				break;

			case around == [0, 0, 0, 1, 0, 1, 1, 0] || around == [0, 0, 0, 1, 0, 1, 1, 1]:
				newTile = 4;
				break;

			case around == [0, 1, 1, 0, 1, 0, 1, 1] || around == [1, 1, 1, 0, 1, 0, 1, 1] || around == [0, 1, 1, 0, 1, 1, 1, 1] || around == [1, 1, 1, 0, 1, 1, 1, 1]:
				newTile = 5;
				break;

			case around == [1, 1, 0, 1, 0, 1, 1, 0]:
				newTile = 6;
				break;

			case around == [0, 1, 1, 0, 1, 0, 0, 0]:
				newTile = 7;
				break;

			case around == [1, 1, 1, 1, 1, 0, 0, 0] || around == [1, 1, 1, 1, 1, 1, 0, 0] || around == [1, 1, 1, 1, 1, 0, 0, 1] || around == [1, 1, 1, 1, 1, 1, 0, 1]:

				newTile = 8;
				break;

			case around == [1, 1, 0, 1, 0, 0, 0, 0]:
				newTile = 9;
				break;

			case around == [0, 0, 0, 0, 1, 0, 0, 0]:
				newTile = 10;
				break;

			case around == [0, 0, 0, 1, 1, 0, 0, 0]:
				newTile = 11;
				break;

			case around == [0, 0, 0, 1, 0, 0, 0, 0]:
				newTile = 12;
				break;

			case around == [1, 1, 1, 1, 1, 0, 1, 1]:
				newTile = 16;
				break;

			case around == [1, 1, 1, 1, 1, 1, 1, 0]:
				newTile = 17;
				break;

			case around == [0, 1, 1, 1, 1, 1, 1, 1]:
				newTile = 18;
				break;

			case around == [1, 1, 0, 1, 1, 1, 1, 1]:
				newTile = 19;
				break;

			default:
				newTile = 1;
				break;
		}
		*/

		//10--15
		if (around[1] == 0 && around[6] == 0)
		{
			if (around[0] == 0 && around[3] == 0 && around[5] == 0)
				newTile = 10;
			else if (around[2] == 0 && around[4] == 0 && around[7] == 0)
				newTile = 12;
			else if (around[3] == 1 && around[4] == 1)
				newTile = 11;
		}
		else if (around[3] == 0 && around[4] == 0)
		{
			if (around[0] == 0 && around[1] == 0 && around[2] == 0)
				newTile = 13;
			else if (around[5] == 0 && around[6] == 0 && around[7] == 0)
				newTile = 15;
			else if (around[1] == 1 && around[6] == 1)
				newTile = 14;
		}

		return newTile;
	}

	private void SaveLevel()
	{
		//PlayerPrefsX.SetIntArray("mapX", map[0]);
		//PlayerPrefsX.SetIntArray("mapY", map[1]);
	}

	private void LoadLevel()
	{
		//int[] newMapX = PlayerPrefsX.GetIntArray("mapX");
		//int[] newMapY = PlayerPrefsX.GetIntArray("mapY");

		//map = [newMapX, newMapY];

		MakeSprites();
	}

	private void MakeSprites()
	{
		Debug.Log("making sprites");

		GameObject newTile;

		for (var x = 0; x < sizeX; x++)
		{
			for (var y = 0; y < sizeY; y++)
			{
				if (map[x, y] > 0)
				{
					newTile = Instantiate(tilePrefab, new Vector3(x, y, 0), Quaternion.identity);
					newTile.GetComponent<SpriteRenderer>().sprite = tileSprites[map[x, y] - 1];

					if (map[x, y] == 17 || map[x, y] == 19)
						newTile.transform.localScale = new Vector3(-1, 1, 1);
				}
			}
		}
	}
}