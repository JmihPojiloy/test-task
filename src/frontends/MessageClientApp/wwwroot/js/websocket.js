document.addEventListener("DOMContentLoaded", function () {
  var socket = new WebSocket("ws://localhost:5290/api/websocket");

  socket.onmessage = function (event) {
    try {
      var message = JSON.parse(event.data);
      displayMessage(message);
    } catch (error) {
      console.error("Error parsing WebSocket message: ", error);
    }
  };

  socket.onclose = function (event) {
    console.log("WebSocket connection closed.");
  };

  socket.onerror = function (error) {
    console.log("WebSocket error: ", error);
  };

  function displayMessage(message) {
    if (message && message.Id && message.Timestamp && message.Content) {
      var messageDate = new Date(message.Timestamp);

      var day = messageDate.getDate().toString().padStart(2, "0");
      var month = (messageDate.getMonth() + 1).toString().padStart(2, "0");
      var year = messageDate.getFullYear();
      var hours = messageDate.getHours().toString().padStart(2, "0");
      var minutes = messageDate.getMinutes().toString().padStart(2, "0");
      var formattedDateTime = `${day}:${month}:${year} ${hours}:${minutes}`;

      var messageList = document.getElementById("message-list");
      var listItem = document.createElement("li");
      listItem.textContent = `Number: ${message.Id} Date: ${formattedDateTime}, Content: ${message.Content}`;
      messageList.appendChild(listItem);
    } else {
      console.error("Received message is missing required fields: ", message);
    }
  }
});
