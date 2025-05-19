document.addEventListener('DOMContentLoaded', function () {
    const userId = document.getElementById('userId').value;
    console.log('User ID:', userId); // Debugging userId
    const dropdownToggle = document.getElementById('navbarDropdownMenuLink');
    const dropdownMenu = document.getElementById('notificationList');

    fetch(`http://localhost:5023/api/Notifications/top3/${userId}`)
        .then(response => response.json())
        .then(data => {
            console.log('Fetched data:', data); // Debugging fetched data
            const notificationList = document.getElementById('notificationList');
            const notificationCount = document.getElementById('notificationCount');

            // Clear existing notifications except the "Show all notifications" link
            notificationList.innerHTML = '<li class="show-all"><a class="dropdown-item" style="text-align: center;color: blue;" href="/Notification/Index">Show all notifications</a></li>';

            // Add new notifications
            data.forEach(notification => {
                const listItem = document.createElement('li');
                const link = document.createElement('a');
                link.className = 'dropdown-item';
                link.textContent = `${notification.NotificationMessage} (${notification.Type})`;
                listItem.appendChild(link);
                notificationList.insertBefore(listItem, notificationList.firstChild);
            });

            // Update the notification count
            notificationCount.textContent = data.length;
        })
        .catch(error => console.error('Error fetching notifications:', error));

    // Toggle dropdown manually
    dropdownToggle.addEventListener('click', function (event) {
        event.preventDefault();
        dropdownMenu.classList.toggle('show');
    });

    // Close dropdown when clicking outside
    document.addEventListener('click', function (event) {
        if (!dropdownToggle.contains(event.target) && !dropdownMenu.contains(event.target)) {
            dropdownMenu.classList.remove('show');
        }
    });
});
