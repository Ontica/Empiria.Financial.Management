/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Contracts Management                       Component : Domain Layer                            *
*  Assembly : Empiria.Contracts.Core.dll                 Pattern   : Infomation Holder                       *
*  Type     : ContractUnit                               License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Represents a contract unit.                                                                    *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.Contracts {

  /// <summary>Represents a contract unit.</summary>
  public class ContractUnit : GeneralObject {

    #region Constructors and parsers

    static internal ContractUnit Parse(string uid) {
      return ParseKey<ContractUnit>(uid);
    }


    static internal ContractUnit Parse(int id) {
      return ParseId<ContractUnit>(id);
    }


    static internal FixedList<ContractUnit> GetList() {
      return GetList<ContractUnit>().ToFixedList();
    }


    static internal ContractUnit Empty => ParseEmpty<ContractUnit>();

    #endregion Constructors and parsers

  }  // class ContractUnit

}  // namespace Empiria.Contracts
