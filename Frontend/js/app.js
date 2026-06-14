let departments = [];
let currentEmployees = [];
let employeeModalInstance = null;
let globalToastInstance = null;
let toastElement = null;

let currentPage = 1;
let pageSize = 10;
let totalRecords = 0;

let totalPages = 1;
let hasNext = false;
let hasPrevious = false;

document.addEventListener("DOMContentLoaded", async () => {
  employeeModalInstance = new bootstrap.Modal(
    document.getElementById("employeeModal"),
  );
  departmentModalInstance = new bootstrap.Modal(
    document.getElementById("departmentModal"),
  );
  toastElement = document.getElementById("globalToast");
  globalToastInstance = new bootstrap.Toast(toastElement);
  departments = await api.getDepartments();
  populateDepartmentDropdown(); // Populates the Modal form
  populateFilterDropdown(); // NEW: Populates the Search Filter

  await loadDepartments();
  await loadEmployees();

  let timeout;
  document.getElementById("searchInput").addEventListener("input", () => {
    clearTimeout(timeout);
    timeout = setTimeout(() => triggerSearch(), 500);
  });
});

function showToast(message, type = "success") {
  const toastMessage = document.getElementById("toastMessage");
  const toastIcon = document.getElementById("toastIcon");

  toastMessage.innerText = message;

  toastElement.classList.remove("text-bg-danger", "text-bg-success");

  if (type === "success") {
    toastElement.classList.add("text-bg-success");
    toastIcon.innerHTML = `<svg xmlns="http://www.w3.org/2000/svg" width="18" height="18" fill="currentColor" class="bi bi-check-circle-fill" viewBox="0 0 16 16"><path d="M16 8A8 8 0 1 1 0 8a8 8 0 0 1 16 0zm-3.97-3.03a.75.75 0 0 0-1.08.022L7.477 9.417 5.384 7.323a.75.75 0 0 0-1.06 1.06L6.97 11.03a.75.75 0 0 0 1.079-.02l3.992-4.99a.75.75 0 0 0-.01-1.05z"/></svg>`;
  } else if (type === "error") {
    toastElement.classList.add("text-bg-danger");
    toastIcon.innerHTML = `<svg xmlns="http://www.w3.org/2000/svg" width="18" height="18" fill="currentColor" class="bi bi-exclamation-triangle-fill" viewBox="0 0 16 16"><path d="M8.982 1.566a1.13 1.13 0 0 0-1.96 0L.165 13.233c-.457.778.091 1.767.98 1.767h13.713c.889 0 1.438-.99.98-1.767L8.982 1.566zM8 5c.535 0 .954.462.9.995l-.35 3.507a.552.552 0 0 1-1.1 0L7.1 5.995A.905.905 0 0 1 8 5zm.002 6a1 1 0 1 1 0 2 1 1 0 0 1 0-2z"/></svg>`;
  }

  globalToastInstance.show();
}
function showPage(pageId) {
  document.querySelectorAll(".view-section").forEach((section) => {
    section.classList.add("d-none");
  });
  document.getElementById(`${pageId}-page`).classList.remove("d-none");

  document.querySelectorAll(".nav-link").forEach((link) => {
    link.classList.remove(
      "bg-info",
      "text-dark",
      "rounded-pill",
      "px-4",
      "py-1",
    );
    link.classList.add("text-light");
  });

  const activeLink = document.querySelector(
    `[onclick="showPage('${pageId}')"]`,
  );
  activeLink.classList.remove("text-light");
  activeLink.classList.add(
    "bg-info",
    "text-dark",
    "rounded-pill",
    "px-4",
    "py-1",
  );
}

function updatePaginationText() {
  const pageInfo = document.getElementById("pageInfo");

  // Handle empty state
  if (totalRecords === 0) {
    pageInfo.innerText = "Showing 0 entries";
    return;
  }

  // Calculate the numbers
  const startItem = (currentPage - 1) * pageSize + 1;
  const endItem = Math.min(currentPage * pageSize, totalRecords);

  // Update the DOM
  pageInfo.innerText = `Showing ${startItem} to ${endItem} of ${totalRecords} entries`;
}

async function loadEmployees(searchTerm = "", departmentId = "") {
  const tbody = document.getElementById("employeeTableBody");
  tbody.innerHTML = `<tr><td colspan="7" class="text-center py-5"><div class="spinner-border text-info" role="status"></div></td></tr>`;

  try {
    const responseData = await api.getEmployees(
      searchTerm,
      departmentId,
      currentPage,
    );

    currentEmployees = responseData.items;
    totalRecords = responseData.totalCount;
    currentPage = responseData.currentPage;
    pageSize = responseData.pageSize;
    totalPages = responseData.totalPages;
    hasNext = responseData.hasNext;
    hasPrevious = responseData.hasPrevious;

    // Render UI
    renderEmployees(currentEmployees);
    updatePaginationText();
    updatePaginationButtons();
  } catch (error) {
    tbody.innerHTML = `<tr><td colspan="7" class="text-center py-4 text-danger">Failed to load data.</td></tr>`;
    showToast("Error loading employees", "error");
  }
}

function renderEmployees(employees) {
  const tbody = document.getElementById("employeeTableBody");
  tbody.innerHTML = ""; // Clear table

  if (employees.length === 0) {
    tbody.innerHTML = `<tr><td colspan="7" class="text-center py-4 text-secondary">No employees found.</td></tr>`;
    return;
  }

  employees.forEach((emp) => {
    const statusBadge = emp.isActive
      ? `<span class="badge bg-success bg-opacity-25 text-success rounded-pill px-3 py-2">Active</span>`
      : `<span class="badge bg-secondary bg-opacity-25 text-secondary rounded-pill px-3 py-2">Inactive</span>`;

    const row = document.createElement("tr");
    row.innerHTML = `
            <td class="ps-4 text-secondary">${emp.id}</td>
            <td class="fw-medium text-light">${emp.fullName}</td>
            <td class="text-secondary">${emp.email}</td>
            <td class="text-light">${emp.departmentName}</td>
            <td class="text-light">${emp.jobTitle}</td>
            <td>${statusBadge}</td>
            <td class="pe-4 text-end">
                <button class="btn btn-sm btn-outline-info rounded-pill px-3 me-2" onclick="openEditModal(${emp.id})">Edit</button>
                <button class="btn btn-sm btn-outline-danger rounded-pill px-3" onclick="deleteEmployee(${emp.id})">Delete</button>
            </td>
        `;
    tbody.appendChild(row);
  });
}

async function loadDepartments() {
  const tbody = document.getElementById("departmentTableBody");
  // Show spinner while loading
  tbody.innerHTML = `<tr><td colspan="4" class="text-center py-5"><div class="spinner-border text-info" role="status"></div></td></tr>`;

  departments = await api.getDepartments();

  // Refresh the dropdown in the Employee form just in case a department was added/removed
  populateDepartmentDropdown();

  tbody.innerHTML = "";

  if (departments.length === 0) {
    tbody.innerHTML = `<tr><td colspan="4" class="text-center py-4 text-secondary">No departments found.</td></tr>`;
    return;
  }

  departments.forEach((dept) => {
    const row = document.createElement("tr");
    row.innerHTML = `
            <td class="ps-4 text-secondary">${dept.id}</td>
            <td class="fw-medium text-light">${dept.name}</td>
            <td class="text-secondary">${dept.description || "-"}</td>
            <td class="pe-4 text-end">
                <button class="btn btn-sm btn-outline-info rounded-pill px-3 me-2" onclick="openEditDeptModal(${dept.id})">Edit</button>
                <button class="btn btn-sm btn-outline-danger rounded-pill px-3" onclick="deleteDepartment(${dept.id})">Delete</button>
            </td>
        `;
    tbody.appendChild(row);
  });
}

function populateDepartmentDropdown() {
  const select = document.getElementById("empDepartment");
  select.innerHTML = '<option value="">Select Department...</option>';
  departments.forEach((dept) => {
    select.innerHTML += `<option value="${dept.id}">${dept.name}</option>`;
  });
}

// --- MODAL & FORM LOGIC ---
function openAddModal() {
  document.getElementById("formErrorSummary").classList.add("d-none");
  document.getElementById("formErrorList").innerHTML = "";
  document.getElementById("employeeForm").reset();
  document.getElementById("employeeForm").classList.remove("was-validated");
  document.getElementById("empId").value = "0";
  document.getElementById("modalTitle").innerText = "Add New Employee";
}

function openEditModal(id) {
  document.getElementById("formErrorSummary").classList.add("d-none");
  document.getElementById("formErrorList").innerHTML = "";

  const emp = currentEmployees.find((e) => e.id === id);
  if (!emp) return;

  document.getElementById("employeeForm").classList.remove("was-validated");
  document.getElementById("empId").value = emp.id;
  document.getElementById("empName").value = emp.fullName;
  document.getElementById("empEmail").value = emp.email;
  document.getElementById("empMobile").value = "";
  document.getElementById("empTitle").value = emp.jobTitle;
  document.getElementById("empDepartment").value = departments.find(
    (d) => d.name === emp.departmentName,
  )?.id;
  document.getElementById("empIsActive").checked = emp.isActive;

  document.getElementById("modalTitle").innerText = "Edit Employee";
  employeeModalInstance.show();
}

// --- CRUD OPERATIONS ---
async function saveEmployee() {
  const form = document.getElementById("employeeForm");
  if (!form.checkValidity()) {
    form.classList.add("was-validated");
    return;
  }

  const employeeData = {
    id: parseInt(document.getElementById("empId").value),
    fullName: document.getElementById("empName").value,
    email: document.getElementById("empEmail").value,
    phoneNumber: document.getElementById("empMobile").value,
    jobTitle: document.getElementById("empTitle").value,
    departmentId: parseInt(document.getElementById("empDepartment").value),
    isActive: document.getElementById("empIsActive").checked,
  };
  const saveBtn = document.querySelector("#employeeModal .btn-info");
  const originalText = saveBtn.innerText;
  saveBtn.innerText = "Saving...";
  saveBtn.disabled = true;

  try {
    await api.saveEmployee(employeeData);

    employeeModalInstance.hide();
    await loadEmployees(document.getElementById("searchInput").value);
    const actionMsg =
      employeeData.id === 0
        ? "Employee added successfully!"
        : "Employee updated successfully!";
    showToast(actionMsg, "success");
  } catch (error) {
    document.getElementById("formErrorSummary").classList.add("d-none");
    document.getElementById("formErrorList").innerHTML = "";
    if (error.isValidationError && error.data.errors) {
      const errorSummary = document.getElementById("formErrorSummary");
      const errorList = document.getElementById("formErrorList");

      errorList.innerHTML = "";

      Object.values(error.data.errors).forEach((errorArray) => {
        errorArray.forEach((msg) => {
          errorList.innerHTML += `<li>${msg}</li>`;
        });
      });

      errorSummary.classList.remove("d-none");
    } else {
      showToast(error.message || "Failed to save the employee.", "error");
    }
  } finally {
    saveBtn.innerText = originalText;
    saveBtn.disabled = false;
  }
}

async function deleteEmployee(id) {
  if (confirm("Are you sure you want to remove this employee?")) {
    try {
      await api.deleteEmployee(id);
      await loadEmployees(document.getElementById("searchInput").value);
      showToast("Employee deleted successfully!", "success");
    } catch (error) {
      showToast(
        error.message || "An error occurred while deleting the employee.",
        "error",
      );
    }
  }
}

// --- DEPARTMENT MODAL LOGIC ---
function openAddDeptModal() {
  document.getElementById("departmentForm").reset();
  document.getElementById("departmentForm").classList.remove("was-validated");
  document.getElementById("deptId").value = "0";
  document.getElementById("deptModalTitle").innerText = "Add New Department";
}

function openEditDeptModal(id) {
  const dept = departments.find((d) => d.id === id);
  if (!dept) return;

  document.getElementById("departmentForm").classList.remove("was-validated");
  document.getElementById("deptId").value = dept.id;
  document.getElementById("deptName").value = dept.name;
  document.getElementById("deptDescription").value = dept.description || "";

  document.getElementById("deptModalTitle").innerText = "Edit Department";
  departmentModalInstance.show();
}

// --- DEPARTMENT CRUD LOGIC ---

function populateFilterDropdown() {
  const filterSelect = document.getElementById("filterDepartment");
  filterSelect.innerHTML = '<option value="">All Departments</option>';

  departments.forEach((dept) => {
    filterSelect.innerHTML += `<option value="${dept.id}">${dept.name}</option>`;
  });
}
async function saveDepartment() {
  const form = document.getElementById("departmentForm");

  // 1. Validate Form
  if (!form.checkValidity()) {
    form.classList.add("was-validated");
    return;
  }

  // 2. Build DTO
  const deptData = {
    id: parseInt(document.getElementById("deptId").value),
    name: document.getElementById("deptName").value,
    description: document.getElementById("deptDescription").value,
  };

  // 3. UI Loading State
  const saveBtn = document.querySelector("#departmentModal .btn-info");
  const originalText = saveBtn.innerText;
  saveBtn.innerText = "Saving...";
  saveBtn.disabled = true;

  try {
    // 4. Attempt API Call
    await api.saveDepartment(deptData);

    // 5. Success: Hide modal and reload grid
    departmentModalInstance.hide();
    await loadDepartments();
    const actionMsg =
      deptData.id === 0 ? "Department added!" : "Department updated!";
    showToast(actionMsg, "success");
  } catch (error) {
    // 6. Fail: Show global error toast
    showToast(error.message || "Failed to save the department.", "error");
  } finally {
    // 7. Cleanup: Always reset the button state, win or lose
    saveBtn.innerText = originalText;
    saveBtn.disabled = false;
  }
}

async function deleteDepartment(id) {
  const hasEmployees = currentEmployees.some((e) => e.departmentId === id);
  if (hasEmployees) {
    showToast(
      "Cannot delete: There are employees assigned to this department.",
      "error",
    );
    return;
  }

  if (confirm("Are you sure you want to remove this department?")) {
    try {
      await api.deleteDepartment(id);

      await loadDepartments();

      showToast("Department deleted successfully!", "success");
    } catch (error) {
      showToast(error.message || "Failed to delete the department.", "error");
    }
  }
}

async function triggerSearch() {
  const searchTerm = document.getElementById("searchInput").value;
  const deptId = document.getElementById("filterDepartment").value;

  await loadEmployees(searchTerm, deptId);
}

function updatePaginationButtons() {
  const prevBtn = document.getElementById("prevBtn");
  const nextBtn = document.getElementById("nextBtn");

  if (prevBtn && nextBtn) {
    prevBtn.disabled = !hasPrevious;
    nextBtn.disabled = !hasNext;
  }
}

async function prevPage() {
  if (hasPrevious) {
    currentPage--;
    await triggerSearch();
  }
}

async function nextPage() {
  if (hasNext) {
    currentPage++;
    await triggerSearch();
  }
}

async function triggerSearch() {
  if (event && (event.type === "input" || event.type === "change")) {
    currentPage = 1;
  }

  const searchTerm = document.getElementById("searchInput").value;
  const deptId = document.getElementById("filterDepartment").value;

  await loadEmployees(searchTerm, deptId);
}
