const body = document.querySelector("body");
const sidebar = document.querySelector(".sidebar");
const mainContent = document.querySelector(".main-content");
const sidebarOpen = document.querySelector("#sidebarOpen");
const sidebarClose = document.querySelector(".collapse_sidebar");
const sidebarExpand = document.querySelector(".expand_sidebar");
const darkLight = document.querySelector("#darkLight");
const submenuItems = document.querySelectorAll(".submenu_item");


function adjustMainContent() {
    if (sidebar.classList.contains("close")) {
        mainContent.style.marginLeft = "80px";
    } else {
        mainContent.style.marginLeft = "260px";
    }
}

sidebarOpen.addEventListener("click", () => {
    sidebar.classList.toggle("close");
    adjustMainContent();
});

sidebarClose.addEventListener("click", () => {
    sidebar.classList.add("close", "hoverable");
    adjustMainContent();
});

sidebarExpand.addEventListener("click", () => {
    sidebar.classList.remove("close", "hoverable");
    adjustMainContent();
});

sidebar.addEventListener("mouseenter", () => {
    if (sidebar.classList.contains("hoverable")) {
        sidebar.classList.remove("close");
        adjustMainContent();
    }
});

sidebar.addEventListener("mouseleave", () => {
    if (sidebar.classList.contains("hoverable")) {
        sidebar.classList.add("close");
        adjustMainContent();
    }
});

darkLight.addEventListener("click", () => {
    body.classList.toggle("dark");
    if (body.classList.contains("dark")) {
        darkLight.classList.replace("bx-sun", "bx-moon");
    } else {
        darkLight.classList.replace("bx-moon", "bx-sun");
    }
});

submenuItems.forEach((item, index) => {
    item.addEventListener("click", () => {
        item.classList.toggle("show_submenu");
        submenuItems.forEach((item2, index2) => {
            if (index !== index2) {
                item2.classList.remove("show_submenu");
            }
        });
    });
});

if (window.innerWidth < 768) {
    sidebar.classList.add("close");
} else {
    sidebar.classList.remove("close");
}

adjustMainContent();

