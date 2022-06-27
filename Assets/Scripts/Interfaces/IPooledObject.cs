using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPooledObject
{
   string PoolType { get; set;}
   int PoolId { get; set;}
}
