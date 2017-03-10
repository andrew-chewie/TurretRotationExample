

using UnityEngine;

public class Turret : MonoBehaviour
{
    public float RotationSpeed;

    public Vector2 HorizontalClamp = new Vector2(89f,89f);
    public Vector2 VerticalCalmp = new Vector2(40f,20f);

    public Transform RotationObjectH;
    public Transform RotationObjectV;
    public Transform BarrelEnd;

    public Transform Target;

    void Update()
    {
        Aim();
    }

    private void Aim()
    {
        if(Target == null)
            return;

        if (RotationObjectH == null)
            return;

        if (RotationObjectV == null)
            return;

        //Transform target position to local space
        Vector3 localAimDirection = transform.InverseTransformDirection(Target.position - RotationObjectH.position);

        //remove vertical component to find only horizontal rotation for the base
        Vector3 aimDirectionH = new Vector3(localAimDirection.x, 0, localAimDirection.z);

        Quaternion RotationH = Quaternion.LookRotation(aimDirectionH);

        //Clamp rotation if needed
        float y = RotationH.eulerAngles.y;

        if (RotationH.eulerAngles.y > 180f && 360f - RotationH.eulerAngles.y > HorizontalClamp.x)
        {
            y = 360f - HorizontalClamp.x;
        }
        else if (RotationH.eulerAngles.y < 180f && RotationH.eulerAngles.y > HorizontalClamp.y)
        {
            y = HorizontalClamp.y;
        }

        RotationH.eulerAngles = new Vector3(0f, y, 0f);
        //end of clamp

        //Apply Horizontal barrel rotation over time
        RotationObjectH.localRotation = Quaternion.RotateTowards(RotationObjectH.localRotation, RotationH, RotationSpeed * Time.deltaTime);

        //Find local vertical rotation of the turret barrel
        Vector3 aimDirectionV = RotationObjectH.InverseTransformDirection(Target.position - RotationObjectV.position);

        //remove unneded horizontal X component
        aimDirectionV = new Vector3(0, aimDirectionV.y, aimDirectionV.z);

        Quaternion RotationV = Quaternion.LookRotation(aimDirectionV);

        //Clamp if needed
        float x = RotationV.eulerAngles.x;

        if (RotationV.eulerAngles.x > 180f && 360f - RotationV.eulerAngles.x > VerticalCalmp.y)
        {
            x = 360f - VerticalCalmp.y;
        }
        else if (RotationV.eulerAngles.x < 180f && RotationV.eulerAngles.x > VerticalCalmp.x)
        {
            x = VerticalCalmp.x;
        }

        RotationV.eulerAngles = new Vector3(x, 0f, 0f);
        //end of clamp

        //Apply Vertical barrel rotation over time
        RotationObjectV.localRotation = Quaternion.RotateTowards(RotationObjectV.localRotation, RotationV, RotationSpeed * Time.deltaTime);
    }
}
