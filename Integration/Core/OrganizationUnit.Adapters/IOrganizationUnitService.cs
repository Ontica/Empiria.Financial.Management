/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Integration services                       Component : Adapters Layer                          *
*  Assembly : Empiria.Financial.Integration.Core.dll     Pattern   : Adaptation Interface                    *
*  Type     : ISialOrganizationUnitService               License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Interface used to retrieve oganization unit data from external systems.                        *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

namespace Empiria.Financial.Adapters {

  /// <summary>Interface used to retrieve oganization unit data from external systems.</summary>
  public interface ISialOrganizationUnitService {

    FixedList<ISialOrganizationUnitData> GetOrganizationUnitEntries();

    FixedList<ISialOrganizationUnitEmployeesData> GetOrganizationUnitEmployeesEntries();

    ISialOrganizationUnitEmployeesData TryGetEmployeeNo(string employeeNo);


  }  // ISialOrganizationUnitService

}  // namespace Empiria.Financial.Adapters
