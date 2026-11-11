document.addEventListener("DOMContentLoaded", () => {
  document.querySelectorAll("[data-table-search]").forEach((input) => {
    const table = document.querySelector(input.dataset.tableSearch);

    if (!table) {
      return;
    }

    const rows = Array.from(table.querySelectorAll("tbody tr"));
    const emptyRow = rows.find((row) => row.querySelector("td[colspan]"));
    const emptyCell = emptyRow?.querySelector("td");
    const initialEmptyMessage = emptyCell?.textContent.trim();

    input.addEventListener("input", () => {
      const query = input.value.trim().toLocaleLowerCase();
      let visibleRows = 0;

      rows.forEach((row) => {
        if (row === emptyRow) {
          return;
        }

        const matches = row.textContent.toLocaleLowerCase().includes(query);
        row.hidden = !matches;
        visibleRows += matches ? 1 : 0;
      });

      if (emptyRow) {
        emptyRow.hidden = query.length > 0 && visibleRows > 0;
        if (emptyCell) {
          emptyCell.textContent = query.length > 0
            ? "Không tìm thấy kết quả phù hợp."
            : initialEmptyMessage;
        }
      }

      let noResultsRow = table.querySelector("tbody .search-no-results");

      if (query.length > 0 && visibleRows === 0 && !emptyRow) {
        if (!noResultsRow) {
          noResultsRow = document.createElement("tr");
          noResultsRow.className = "search-no-results";
          noResultsRow.innerHTML = `<td colspan="${table.tHead.rows[0].cells.length}">Không tìm thấy kết quả phù hợp.</td>`;
          table.tBodies[0].appendChild(noResultsRow);
        }
        noResultsRow.hidden = false;
      } else if (noResultsRow) {
        noResultsRow.hidden = true;
      }
    });
  });
});
