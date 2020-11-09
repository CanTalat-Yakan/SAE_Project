using UnityEngine;
using UnityEngine.UI;

public enum FitType
{
    Uniform,
    Width, 
    Height,
    FixedRows,
    FixedColumns
}
public class FlexibleGridLayout : LayoutGroup
{
    #region -variables-
    [SerializeField]
    FitType m_fitType;

    [SerializeField]
    int m_rows, m_columns;

    [SerializeField]
    Vector2 m_cellSize, m_spacing;

    [SerializeField]
    bool m_fitX, m_fitY;
    #endregion

    public override void CalculateLayoutInputHorizontal()
    {
        base.CalculateLayoutInputHorizontal();

        if (m_fitType == FitType.Width || m_fitType == FitType.Height || m_fitType == FitType.Uniform) 
        {
            float sqrRt = Mathf.Sqrt(transform.childCount);
            m_rows = m_columns = Mathf.CeilToInt(sqrRt);
            m_fitX = m_fitY = true;
        }

        if(m_fitType == FitType.Width || m_fitType == FitType.FixedColumns)
            m_rows = Mathf.CeilToInt(transform.childCount / (float)m_columns);

        if(m_fitType == FitType.Height || m_fitType == FitType.FixedRows)
            m_columns = Mathf.CeilToInt(transform.childCount / (float)m_rows);

        float parentWidth = rectTransform.rect.width;
        float parentHeight= rectTransform.rect.height;

        float cellWidth = (parentWidth / (float)m_columns) - ((m_spacing.x / (float)m_columns) * 2) - (padding.left / (float) m_columns) - (padding.right / (float)m_columns);
        float cellHeight = (parentHeight / (float)m_rows) - ((m_spacing.y / (float)m_rows) * 2) - (padding.top / (float) m_rows) - (padding.bottom / (float) m_rows);

        m_cellSize.x = m_fitX ? cellWidth : m_cellSize.x;
        m_cellSize.y = m_fitY ? cellHeight : m_cellSize.y;
        
        int columnCount, rowCount = 0;

        for (int i = 0; i < rectChildren.Count; i++)
        {
            rowCount = i / m_columns;
            columnCount = i % m_columns;

            var item = rectChildren[i];

            var xPos = (m_cellSize.x * columnCount) + (m_spacing.x * columnCount) + padding.left;
            var yPos = (m_cellSize.y * rowCount) + (m_spacing.y * rowCount) + padding.top;

            SetChildAlongAxis(item, 0, xPos, m_cellSize.x);
            SetChildAlongAxis(item, 1, yPos, m_cellSize.y);
        }
    }

    #region -not used-
    public override void CalculateLayoutInputVertical()
    {

    }

    public override void SetLayoutHorizontal()
    {

    }

    public override void SetLayoutVertical()
    {

    }
    #endregion
}
