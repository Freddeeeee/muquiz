(async function () {
    const connection = new signalR.HubConnectionBuilder()
        .withUrl("/gamehub")
        .build();

    await connection.start().catch(err => console.log(err));

    connection.invoke("AddToGroup", )
})();