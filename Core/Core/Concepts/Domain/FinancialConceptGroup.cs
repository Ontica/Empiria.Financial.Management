/* Empiria Financial  ****************************************************************************************
*                                                                                                            *
*  Module   : Financial Concepts                         Component : Domain Layer                            *
*  Assembly : Empiria.Financial.Core.dll                 Pattern   : Information Holder                      *
*  Type     : FinancialConceptGroup                      License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Represents a financial concept group.                                                          *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Financial.Concepts.Data;

namespace Empiria.Financial.Concepts {

  /// <summary>Represents a financial concept group.</summary>
  public class FinancialConceptGroup : CommonStorage {

    #region Constructors and parsers

    protected FinancialConceptGroup() {
      // Required by Empiria Framework
    }

    static internal FinancialConceptGroup Parse(int id) => ParseId<FinancialConceptGroup>(id);

    static internal FinancialConceptGroup Parse(string uid) => ParseKey<FinancialConceptGroup>(uid);

    static public FinancialConceptGroup ParseWithNamedKey(string namedKey) =>
                                                        ParseNamedKey<FinancialConceptGroup>(namedKey);

    static public FinancialConceptGroup Empty => ParseEmpty<FinancialConceptGroup>();

    static public FixedList<FinancialConceptGroup> GetList() {
      return GetStorageObjects<FinancialConceptGroup>();
    }

    #endregion Constructors and parsers

    #region Properties

    public new string NamedKey {
      get {
        return base.NamedKey;
      }
      private set {
        base.NamedKey = value;
      }
    }

    #endregion Properties

    #region Methods

    public FixedList<FinancialConcept> GetConcepts() {
      return FinancialConceptsData.GetConcepts(this);
    }

    #endregion Methods

  } // class FinancialConceptGroup

} // namespace Empiria.Financial.Concepts
