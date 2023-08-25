using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCreation : MonoBehaviour
{
    private List<GameObject> characters;

    //Default Index of the charcter
    private int selectionIndex = 0;

    private void Start()
    {
        characters = new List<GameObject>();
        foreach(Transform t in transform)
        {
            characters.Add(t.gameObject);
            t.gameObject.SetActive(false);
            Debug.Log("うんち");
        }
        characters[selectionIndex].SetActive(true);
    }

    private void Update()
    {

    }


    //キャラクター選択
    public void Select(int index)
    {
       
        //受け取った番号が選択された番号と同じ場合、何も無し
        if (index == selectionIndex)
        {
            return;
        }
        //受け取った番号が0より小さい、
        //または受け取った番号がキャラクターの要素数以上だった時
        if(index < 0 || index >= characters.Count)
        {
            Debug.Log(index);
            Debug.Log(selectionIndex);
            return;
        }
        characters[selectionIndex].SetActive(false);
        selectionIndex = index;
        characters[selectionIndex].SetActive(true);
        //Debug.Log("変更されました");
    }
}
