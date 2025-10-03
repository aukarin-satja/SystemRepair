

    const datamonth = {
        labels: monthLabels,
    datasets: [{
        label: 'จำนวนรายการที่ซ่อมต่อเดือน',
    data:MonthsData,
    backgroundColor: [
    'rgba(255, 99, 132, 0.2)',
    'rgba(255, 159, 64, 0.2)',
    'rgba(255, 205, 86, 0.2)',
    'rgba(75, 192, 192, 0.2)',
    'rgba(54, 162, 235, 0.2)',
    'rgba(153, 102, 255, 0.2)',
    'rgba(201, 203, 207, 0.2)'
    ],
    borderColor: [
    'rgb(255, 99, 132)',
    'rgb(255, 159, 64)',
    'rgb(255, 205, 86)',
    'rgb(75, 192, 192)',
    'rgb(54, 162, 235)',
    'rgb(153, 102, 255)',
    'rgb(201, 203, 207)'
    ],
    borderWidth: 1
      }]
    };

    const configMonth = {
        type: 'bar',
    data: datamonth,
    options: {
        responsive: true,
    maintainAspectRatio: false,
    scales: {
        y: {
        beginAtZero: true,
    ticks: {
        stepSize: 20
            }
          }
        },
    plugins: {
        legend: {
        display: true,
    position: 'top',
    labels: {
        font: {
        size: 12
              }
            }
          },
    tooltip: {
        enabled: true
          }
        }
      }
    };

    new Chart(document.getElementById('monthChart'), configMonth);
