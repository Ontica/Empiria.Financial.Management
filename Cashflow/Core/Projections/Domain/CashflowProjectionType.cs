/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Cash flow Management                       Component : Domain Layer                            *
*  Assembly : Empiria.CashFlow.Core.dll                  Pattern   : Power type                              *
*  Type     : CashFlowProjectionType                     License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Power type that describes a cash flow projection.                                              *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System.Linq;

using Empiria.Ontology;
using Empiria.Parties;

namespace Empiria.CashFlow.Projections {

  /// <summary>Power type that describes a cash flow projection.</summary>
  [Powertype(typeof(CashFlowProjection))]
  public sealed class CashFlowProjectionType : Powertype {

    #region Constructors and parsers

    private CashFlowProjectionType() {
      // Empiria power types always have this constructor.
    }

    static public new CashFlowProjectionType Parse(int typeId) => Parse<CashFlowProjectionType>(typeId);

    static public new CashFlowProjectionType Parse(string typeName) => Parse<CashFlowProjectionType>(typeName);

    static public FixedList<CashFlowProjectionType> GetList() {
      return Empty.GetAllSubclasses()
            .Select(x => (CashFlowProjectionType) x)
            .ToFixedList();
    }

    static public CashFlowProjectionType Empty => Parse("ObjectTypeInfo.CashFlowProjection");

    #endregion Constructors and parsers

    #region Properties

    public bool IsProtected {
      get {
        return ExtensionData.Get("isProtected", false);
      }
    }


    public FixedList<OperationSource> Sources {
      get {
        return ExtensionData.GetFixedList<OperationSource>("sources", false)
                            .Sort((x, y) => x.Name.CompareTo(y.Name));
      }
    }


    public string Prefix {
      get {
        return ExtensionData.Get("prefix", string.Empty);
      }
    }


    public FixedList<ObjectTypeInfo> RelatedDocumentTypes {
      get {
        var ids = ExtensionData.GetFixedList<int>("relatedDocumentTypes", false);

        return ids.Select(x => ObjectTypeInfo.Parse(x))
                  .ToFixedList();
      }
    }

    #endregion Properties

  } // class CashFlowProjectionType

}  // namespace Empiria.CashFlow.Projections
