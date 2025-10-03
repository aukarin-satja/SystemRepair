
const config = {
    type: 'doughnut',
    data: {
        labels: labels,
        datasets: [{
            label: 'จำนวนรายการซ่อม',
            data: data,
            backgroundColor: [
                'rgb(255, 99, 132)',
                'rgb(54, 162, 235)',
                'rgb(255, 205, 86)',
                'rgb(75, 192, 192)',
                'rgb(153, 102, 255)',
                'rgb(255, 159, 64)'
            ],
            hoverOffset: 4
        }]
    },
    options: {
        responsive: true,
        maintainAspectRatio: false
    }
};


new Chart(document.getElementById('DonutChart'), config);