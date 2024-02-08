using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu]
public class SiblingTileRule : RuleTile
{

    public enum SiblingGroup
    {       
        Earth,
        Rock,
        Sand,
        Snow,
        Stone_brick,
        Sand_house,
        Snow_house,
        Stone_house,
        Wood_house,
    }

    public SiblingGroup sibingGroup;

    public override bool RuleMatch(int neighbor, TileBase other)
    {
        if (other is RuleOverrideTile)
            other = (other as RuleOverrideTile).m_InstanceTile;

        switch (neighbor)
        {
            case TilingRule.Neighbor.This:
                {
                    return other is SiblingTileRule
                        && (other as SiblingTileRule).sibingGroup == this.sibingGroup;
                }
            case TilingRule.Neighbor.NotThis:
                {
                    return !(other is SiblingTileRule
                        && (other as SiblingTileRule).sibingGroup == this.sibingGroup);
                }
        }
        return base.RuleMatch(neighbor, other);
    }
}