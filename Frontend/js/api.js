const API_BASE_URL = "https://localhost:7040/api";
const delay = (ms) => new Promise((resolve) => setTimeout(resolve, ms));

const api = {
  // --- DEPARTMENTS ---
  async getDepartments() {
    const response = await fetch(`${API_BASE_URL}/department`);
    if (!response.ok) throw new Error("Failed to load departments.");
    return await response.json();
  },

  async saveDepartment(department) {
    const isUpdate = department.id !== 0;
    const url = isUpdate
      ? `${API_BASE_URL}/department/${department.id}`
      : `${API_BASE_URL}/department`;
    const method = isUpdate ? "PUT" : "POST";

    const response = await fetch(url, {
      method: method,
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify(department),
    });

    if (!response.ok) throw new Error("Failed to save department.");
    return response.status === 204 ? department : await response.json();
  },

  async deleteDepartment(id) {
    const response = await fetch(`${API_BASE_URL}/department/${id}`, {
      method: "DELETE",
    });
    if (!response.ok) throw new Error("Failed to delete department.");
    return true;
  },

  async getEmployees(searchTerm = "", departmentId = "", pageNumber = 1) {
    const params = new URLSearchParams();

    if (searchTerm) params.append("SearchTerm", searchTerm);
    if (departmentId) params.append("DepartmentId", departmentId);

    params.append("PageNumber", pageNumber);

    const queryString = params.toString();
    const url = queryString
      ? `${API_BASE_URL}/employee?${queryString}`
      : `${API_BASE_URL}/employee`;

    const response = await fetch(url);
    if (!response.ok) throw new Error("Failed to load employees.");
    return await response.json();
  },

  async saveEmployee(employee) {
    const isUpdate = employee.id !== 0;
    const url = isUpdate
      ? `${API_BASE_URL}/employee/${employee.id}`
      : `${API_BASE_URL}/employee`;
    const method = isUpdate ? "PUT" : "POST";

    const response = await fetch(url, {
      method: method,
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify(employee),
    });

    if (!response.ok) {
      if (response.status === 400) {
        const errorData = await response.json();
        throw { isValidationError: true, data: errorData };
      }
      throw new Error("Failed to save employee.");
    }

    return response.status === 204 ? employee : await response.json();
  },

  async deleteEmployee(id) {
    const response = await fetch(`${API_BASE_URL}/employee/${id}`, {
      method: "DELETE",
    });
    if (!response.ok) throw new Error("Failed to delete employee.");
    return true;
  },
};
