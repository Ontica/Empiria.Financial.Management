/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Financial cost object                      Component : Domain Layer                            *
*  Assembly : Empiria.Financial.Core.dll                 Pattern   : Partitioned Type                        *
*  Type     : Financial                                  License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Represents a financial cost objects.                                                           *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;
using Empiria.Ontology;

namespace Empiria.Financial {

  /// <summary>Represents a financial cost objects.</summary>
  public class FinancialCostObjectType : CommonStorage {

    #region Constructors and parsers

    protected FinancialCostObjectType() {
      // Required by Empiria Framework
    }

    static internal FinancialCostObjectType Parse(int id) => ParseId<FinancialCostObjectType>(id);

    static internal FinancialCostObjectType Parse(string uid) => ParseKey<FinancialCostObjectType>(uid);

    static public FinancialCostObjectType ParseWithNamedKey(string namedKey) => ParseNamedKey<FinancialCostObjectType>(namedKey);

    static public FinancialCostObjectType Empty => ParseEmpty<FinancialCostObjectType>();

    static public FixedList<FinancialCostObjectType> GetList() {
      return GetStorageObjects<FinancialCostObjectType>();
    }

    #endregion Constructors and parsers

  } // <summary>Represents a financial cost objects.</summary>

} // FinancialCostObjectType