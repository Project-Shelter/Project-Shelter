using UnityEngine;

// HP를 회복할 수 있는 타입들이 공통적으로 가져야 하는 인터페이스
public interface IRestoreable
{
    void RestoreHP(float restoreHP);
}
