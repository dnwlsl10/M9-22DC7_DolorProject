using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// === 방패의 기능 ===
// 적의 투사체 모든 것을 막는다.
// 방패의 체력
// 투사체 별 - 데미지

// 게임 시작 시 게이지 100
// Down
// LeftIndexTrigger 버튼을 누르면 방패 오브젝트가 켜지고
// 

// 애니메이션의 상태를 ShieldOn = Play
// 누르고 있으면 게이지가 초당 -1씩 깎이고
//

//떼면 게이지가 초당 1씩 회복되고
//애니메이션의 상태를 초기화

public class SkillShield : MonoBehaviour
{
    public GameObject shieldCreatePos;
    public GameObject _Shield;


    // Start is called before the first frame update
    void Start()
    {
        _Shield.SetActive(false);
        _Shield.transform.position = shieldCreatePos.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        StartWeaponAction();
    }
    public void StartWeaponAction()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            _Shield.SetActive(true);
        }
        if (Input.GetKeyUp(KeyCode.Alpha1))
        {
            _Shield.SetActive(false);
        }
    }
    // public void StopWeaponAction()
    // {
    //     if (Input.GetKeyUp(KeyCode.Alpha1))
    //     {
    //         _Shield.SetActive(false);
    //     }
    // }
}
