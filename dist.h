sudo git -C /home/ubuntu/grey.house pull
sudo systemctl stop grey.house.service
sudo systemctl start grey.house.service
sudo systemctl status grey.house.service 