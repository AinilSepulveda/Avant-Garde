using UnityEngine;

[CreateAssetMenu(fileName = "DropTable", menuName = "DropTable/ItempDrop", order = 0)]
public class DropTable : ScriptableObject
{
    public MobyType mobyType;
    public DropDefinition[] drops;
}



