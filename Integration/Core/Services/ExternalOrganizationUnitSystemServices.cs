/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Integration services                       Component : Domain Layer                            *
*  Assembly : Empiria.Financial.Integration.Core.dll     Pattern   : Service provider                        *
*  Type     : ExternalSialOrganizationUnitSystemServices License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Provides services from external payroll systems.                                               *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

using Empiria.Reflection;

using Empiria.Financial.Adapters;

namespace Empiria.Financial {

  /// <summary>Provides services from external payroll systems.</summary>
  public class ExternalSialOrganizationUnitSystemServices {

    #region Fields

    private readonly ISialOrganizationUnitService _service;

    #endregion Fields

    #region Constructors and parsers

    public ExternalSialOrganizationUnitSystemServices() {
      var typeConfig = ConfigurationData.GetString("ExternalSialOrganizationUnitSystemServices");

      string[] typeData = typeConfig.Split(';');

      Type type = ObjectFactory.GetType(assemblyName: typeData[0],
                                        typeName: typeData[1]);

      _service = (ISialOrganizationUnitService) ObjectFactory.CreateObject(type);

    }

    #endregion Constructors and parsers

    #region Methods

    public FixedList<ISialOrganizationUnitData> GetOrganizationUnitEntries() {


      return _service.GetOrganizationUnitEntries();
    }


    public ISialOrganizationUnitEmployeesData GetOrganizationUnitEmployee(string employeeNo) {


      return _service.TryGetEmployeeNo(employeeNo);
    }

    public FixedList<ISialOrganizationUnitEmployeesData> GetOrganizationUnitEmployeesEntries() {


      return _service.GetOrganizationUnitEmployeesEntries();
    }


    #endregion Methods

  }  // ExternalSialOrganizationUnitSystemServices

}  // namespace Empiria.Financial.Adapters
