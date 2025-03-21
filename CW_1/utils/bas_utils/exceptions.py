from utils.bas_utils.models.base_message import Status
from utils.exceptions import BasicException


class BaseAesException(BasicException):
    """ Базовое исключение AES """

    def __init__(self, status: Status):
        super().__init__(str(status))
        self.status = status


class ConnectionErrorException(BaseAesException):
    """ Ошибка сетевого соединения """


class S3Exception(BaseAesException):
    """ Ошибка, связанная с S3 """


class InternalErrorException(BaseAesException):
    """ Внутренняя ошибка при работе технологий """


class TechHandleException(BaseAesException):
    """ Ошибка при работе технологий """
