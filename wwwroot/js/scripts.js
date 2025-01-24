$(document).ready(function () {
    const batterStatsTable = $('#batterStatsTable').DataTable();
    const pitcherStatsTable = $('#pitcherStatsTable').DataTable();

    $('#batterTab').on('click', function (e) {
        e.preventDefault();
        $('#batterStatsSection').show();
        $('#pitcherStatsSection').hide();
        $(this).addClass('active');
        $('#pitcherTab').removeClass('active');
    });

    $('#pitcherTab').on('click', function (e) {
        e.preventDefault();
        $('#pitcherStatsSection').show();
        $('#batterStatsSection').hide();
        $(this).addClass('active');
        $('#batterTab').removeClass('active');
    });

    function loadStats(table, data) {
        table.clear();
        data.forEach(player => {
            table.row.add([
                player.year || 'N/A',
                player.playerName,
                player.teamName || 'N/A',
                player.gamesPlayed || 0,
                player.AB || 0,
                player.H || 0,
                player.HR || 0,
                player.R || 0,
                player.W || 0,
                player.L || 0,
                player.SO || 0,
                player.IP || 0
            ]);
        });
        table.draw();
    }

    $('#addBatterGameForm').on('submit', function (e) {
        e.preventDefault();
        $.ajax({
            url: '/Home/AddBatterGame',
            type: 'POST',
            data: JSON.stringify({
                playerName: $('#batterPlayerName').val(),
                teamName: $('#batterTeamName').val(),
                gameDate: $('#batterGameDate').val(),
                AB: $('#batterAB').val(),
                H: $('#batterH').val(),
                Doubles: $('#batterDoubles').val(),
                Triples: $('#batterTriples').val(),
                HR: $('#batterHR').val(),
                R: $('#batterR').val()
            }),
            contentType: "application/json",
            success: function (response) {
                alert(response.message);
            },
            error: function () {
                alert('Failed to add batter stats.');
            }
        });
    });

    $('#addPitcherGameForm').on('submit', function (e) {
        e.preventDefault();
        $.ajax({
            url: '/Home/AddPitcherGame',
            type: 'POST',
            data: $(this).serialize(),
            success: function (response) {
                loadStats(pitcherStatsTable, response);
            },
            error: function () {
                alert('Failed to add pitcher stats.');
            }
        });
    });

    $('#loadButton').on('click', function () {
        $.ajax({
            url: '/data/gamedata.json',
            type: 'GET',
            success: function (data) {
                loadStats(batterStatsTable, data);
                loadStats(pitcherStatsTable, data);
            },
            error: function () {
                alert('Failed to load data.');
            }
        });
    });
});

