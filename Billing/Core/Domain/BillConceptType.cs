/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Billing                                    Component : Domain Layer                            *
*  Assembly : Empiria.Billing.Core.dll                   Pattern   : Power type                              *
*  Type     : BillConceptType                            License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Power type that represents a bill concept type like a normal concept or addenda-related.       *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System.Linq;

using Empiria.Ontology;

namespace Empiria.Billing {

  /// <summary>Power type that represents a bill concept type like a normal concept or addenda-related.</summary>
  [Powertype(typeof(BillConcept))]
  public sealed class BillConceptType : Powertype {

    #region Constructors and parsers

    private BillConceptType() {
      // Empiria powertype types always have this constructor.
    }

    static public new BillConceptType Parse(int typeId) => Parse<BillConceptType>(typeId);

    static public new BillConceptType Parse(string typeName) => Parse<BillConceptType>(typeName);

    static public BillConceptType Default => Parse("ObjectTypeInfo.BillConcept");

    static public BillConceptType Addenda => Parse("ObjectTypeInfo.BillConcept.Addenda");

    static public BillConceptType GasStation => Parse("ObjectTypeInfo.BillConcept.GasStation");

    static public FixedList<BillConceptType> GetList() {
      return Default.GetAllSubclasses()
                    .Select(x => (BillConceptType) x)
                    .ToFixedList();
    }

    #endregion Constructors and parsers

  }  // class BillConceptType

}  // namespace Empiria.Billing
