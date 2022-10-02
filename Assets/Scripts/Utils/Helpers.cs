using System.Collections.Generic;
using UnityEngine;

public class Helpers {

    public static T RandomElement<T>(T[] array) {
        return array[Random.Range(0, array.Length)];
    }

    public static T RandomElement<T>(List<T> list) {
        return list[Random.Range(0, list.Count)];
    }
}