<!-- @format -->

# [2D 연습](https://youtu.be/v_Y5FH_tCpc?list=PLO-mt5Iu5TeYI4dbYwWP8JqZMC9iuUIW2)

## 목차

- [Sprite](#sprite)
- [Camera](#camera)
- [Animation](#animation)
- [Project Settings](#project-settings)
- [Core Module](#core-module)
- [Physics 2D Module](#physics-2d-module)
- [Component](#component)
- [Tile Palette](#tile-palette)

---

## Sprite

```
SpriteRenderer :
    스프라이트를 보여주는 컴포넌트
    이미지 색상 변경 가능
    Order in Layer 값이 높을수록 앞에서 렌더링 된다

```

---

## Camera

```
Size를 조절하여 줌인, 줌아웃을 할 수 있다.

Projection :
    투영법을 나타냄
    Orthographic : 원근법이 없는 정사영 투시
    Perspective : 원근법이 있는 투시

```

---

## [Animation](https://youtu.be/Z4iULRbiSTg?list=PLO-mt5Iu5TeYI4dbYwWP8JqZMC9iuUIW2&t=1258)

```
Animator :
    애니메이션을 관리하는 컴포넌트
    애니메이터 매개변수를 기준으로 상태를 바꿀 수 있다.
    Has Exit Time 옵션을 체크시 지금 에니메이션이 끝나기 전까지 안넘어감.

    Transition :
        애니메이션 상태를 옮겨가는 통로
        애니메이션 창에서 우클릭 해서 만들 수 있다.


    Animator.SetBool("parameter name", true) :
        animator에 설정된 해당하는 이름의 논리값을 설정 true 대신 false 가능



```

---

## Project Settings

```
Physics 2D :
    Default Contact Offset :
        충돌 여백

```

---

## [Core Module](https://docs.unity3d.com/ScriptReference/UnityEngine.CoreModule.html)

```
Vector2 :
    Vector2.Normalize() :
        public void Normalize();
        크기가 1인 백터로 변환

    Vector2.Normalized :
        public Vector2 normalized;
        크기가 1인 백터를 반환(Read Only)

SpriteRenderer :
    Renders a Sprite for 2D graphics.

    Flip :
        스프라이트를 뒤집는 옵션

LayerMask :
    물리 효과를 구분하는 거

    LayerMask.GetMask("layerName") :
        해당하는 이름의 레이어의 숫자를 리턴함

Invoke("함수이름", float time) :
    "함수이름" 함수를 time초 뒤에 호출함

CancelInvoke("함수이름") :
    "함수이름"함수의 예약을 취소함
    만약 파라메터가 없음 해당 MonoBehaviour의 모든 예약을 취소


```

---

## [Physics 2D Module](https://docs.unity3d.com/ScriptReference/UnityEngine.PhysicsModule.html)

```
Global settings and helpers for 2D physics.

Rigidbody2D :
    Control of an object's position through physics simulation.

    velocity :
        public Vector2 velocity;

    Freeze Rotation 옵션 :
        체크시 오브젝트가 회전하지 않음

RaycastHit2D :
    Ray에 닿은 오브젝트

Physics2D.Raycast( origin, direction, distance, layerMask ) :
    origin, direction은 Vector2
    distance는 float
    layerMask는 int

    origin에서 direction방향으로 distance만큼 ray를 쏴서 처음 만나는 layerMask에 있는 오브젝트의 정보를 반환

```

---

## [Component](https://docs.unity3d.com/ScriptReference/Component.html)

```
TileMap :
    타일을 일정하게 깔아두는 컴포넌트

TileMap Collider 2D :
    타일맵에 맞춰 생성되는 콜라이더

```

---

## [Tile Palette](https://youtu.be/f8ixw9IpnD8?list=PLO-mt5Iu5TeYI4dbYwWP8JqZMC9iuUIW2)

```
타일을 사용하기 위해 모아둔 프리펩

```
