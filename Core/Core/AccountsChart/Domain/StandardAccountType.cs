/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Financial Projects                         Component : Domain Layer                            *
*  Assembly : Empiria.Financial.Core.dll                 Pattern   : Power type                              *
*  Type     : StandardAccountType                        License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Power type that describes an standard account.                                                 *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System.Linq;

using Empiria.Ontology;

namespace Empiria.Financial {

  /// <summary>Power type that describes an standard account.</summary>
  [Powertype(typeof(StandardAccount))]
  public sealed class StandardAccountType : Powertype {

    #region Constructors and parsers

    private StandardAccountType() {
      // Empiria power types always have this constructor.
    }

    static public new StandardAccountType Parse(int typeId) => Parse<StandardAccountType>(typeId);

    static public new StandardAccountType Parse(string typeName) => Parse<StandardAccountType>(typeName);

    static public FixedList<FinancialAccountType> GetList() {
      return Empty.GetAllSubclasses()
            .Select(x => (FinancialAccountType) x)
            .ToFixedList();
    }

    static public StandardAccountType Empty => Parse("ObjectTypeInfo.StandardAccount");

    #endregion Constructors and parsers

    #region Properties

    //public FinancialAccountType FinancialAccountType {
    //  get {
    //    int id = base.ExtensionData.Get<int>("financialAccountTypeId");

    //    return FinancialAccountType.Parse(id);
    //  }
    //}

    #endregion Properties

  } // class StandardAccountType

}  // namespace Empiria.Financial
