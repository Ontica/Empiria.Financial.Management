/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Cash flow Management                       Component : Domain Layer                            *
*  Assembly : Empiria.Cashflow.Core.dll                  Pattern   : Power type                              *
*  Type     : CashflowProjectionType                     License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Power type that describes a cash flow projection.                                              *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System.Linq;

using Empiria.Ontology;
using Empiria.Parties;

namespace Empiria.Cashflow.Projections {


  /// <summary>Power type that describes a cash flow projection.</summary>
  [Powertype(typeof(CashflowProjection))]
  public sealed class CashflowProjectionType : Powertype {

    #region Constructors and parsers

    private CashflowProjectionType() {
      // Empiria power types always have this constructor.
    }

    static public new CashflowProjectionType Parse(int typeId) => Parse<CashflowProjectionType>(typeId);

    static public new CashflowProjectionType Parse(string typeName) => Parse<CashflowProjectionType>(typeName);

    static public FixedList<CashflowProjectionType> GetList() {
      return Empty.GetAllSubclasses()
            .Select(x => (CashflowProjectionType) x)
            .ToFixedList();
    }

    static public CashflowProjectionType Empty => Parse("ObjectTypeInfo.CashflowProjection");

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

  } // class CashflowProjectionType

}  // namespace Empiria.Cashflow.Projections
