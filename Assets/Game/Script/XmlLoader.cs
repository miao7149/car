using System.Xml;
using UnityEngine;

public class XmlLoader : MonoBehaviour
{
    public string xmlFileName = "Data.xml";

    void Start()
    {
        LoadXmlFile(xmlFileName);
    }

    void LoadXmlFile(string fileName)
    {
        // 加载 XML 文件
        TextAsset xmlAsset = Resources.Load<TextAsset>(fileName);
        if (xmlAsset == null)
        {
            Debug.LogError("无法加载 XML 文件: " + fileName);
            return;
        }

        // 解析 XML 文件
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(xmlAsset.text);

        // 处理 XML 数据
        XmlNodeList worksheetList = xmlDoc.GetElementsByTagName("Worksheet");
        foreach (XmlNode worksheet in worksheetList)
        {
            string sheetName = worksheet.Attributes["Name"].Value;
            XmlNodeList rowList = worksheet.SelectNodes("Row");
            foreach (XmlNode row in rowList)
            {
                XmlNodeList cellList = row.SelectNodes("Cell");
                foreach (XmlNode cell in cellList)
                {
                    string column = cell.Attributes["Column"].Value;
                    string value = cell.InnerText;
                }
            }
        }
    }
}