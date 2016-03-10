using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnControl : MonoBehaviour
{
	public List<GameObject> baddies;
	public List<Transform> spawnPoints;

	public int seconds;

	void Start()
	{
		spawnPoints = new List<Transform> ();
		baddies = new List<GameObject> ();

		SetAsSpawnPoint.PassSpawnPointTransform += AddToSpawnPointsList;
		//SetAsBaddie.PassBaddieGameObject += AddToBaddiesList;
		AddBaddies.AddBaddiesBack += AddToBaddiesList;

	}

	public void StartSpawn()
	{
			StartCoroutine (Spawn ());
			print ("started coroutine");
		
	}

	IEnumerator Spawn()
	{
		print ("waiting");
		do {
			yield return new WaitForSeconds (seconds);
			int n = 1;
			print (n++);

			int random = Random.Range (0, baddies.Count - 1);
			baddies [random].SetActive (true);

			int randomSpawnPointNum = Random.Range (0, spawnPoints.Count - 1);
			baddies [random].transform.position = spawnPoints [randomSpawnPointNum].position;

			if (baddies.Count > 0)
			{
				baddies.RemoveAt (random);
			}
		} while (baddies.Count > 0);
	}


	void AddToSpawnPointsList(Transform _spawnPoint)
	{
		spawnPoints.Add (_spawnPoint);
	}

	void AddToBaddiesList(GameObject _baddie)
	{
		baddies.Add (_baddie);
	}
}